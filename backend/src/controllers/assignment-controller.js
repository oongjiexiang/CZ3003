/**
 * Controller for assignment related logic.
 * @module assignment-controller
 * @category controller
 */

const admin = require('firebase-admin');
const db = admin.firestore();
const AssignmentCollection = db.collection("assignment")
const UsersCollection = db.collection('users');
const AssignmentResultCollection = db.collection("assignmentResult")
const AssignmentQuestionCollection = db.collection("assignmentQuestion")


/**
 * Create assignment and store into the database. Assignment must have necessary field.
 * @param {Object} record - New assignment details, including assignmentName, startDate, dueDate, questions and tries.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.createAssignment = async function(record, callback) {
    if (record['assignmentName'] == null || record['startDate'] == null || record['dueDate'] == null 
                || record['questions'] == null || record['tries'] == null) {
        callback('Missing fields', null)
        return
    }
    try{

        const questionIds = []

        for(let i = 0; i < record['questions'].length; i++){
            const ques = record['questions'][i];
            if (ques['answer'] == null || ques['correctAnswer'] == null  || ques['question'] == null || ques['score'] == null ) {
                callback('Missing fields on question', null)
                return
            }
            const reply = await AssignmentQuestionCollection.add(ques)
            questionIds.push(reply.id)
        }

        record['questions'] = questionIds
        const reply = await AssignmentCollection.add(record)
        callback(null, reply.id)
        
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Update assignment by assignmentId, updated field name must be valid.
 * @param {String} assignmentId - AssignmentId of assignment to be updated.
 * @param {Object} updateMap - Object include new data to update.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.updateAssignment = async function(assignmentId, updateMap, callback){
    try{
        const assignment = await AssignmentCollection.doc(assignmentId).get();
        if(!assignment.exists){
            callback('Asssignment does not exist', null)
        }
        else{
            const res = await AssignmentCollection.doc(assignmentId).update(updateMap)
            callback(null, "Update successfully");
        }
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Add a assignment question to assignment by assignmentId. Assignment question must contains all necessary field.
 * @param {String} assignmentId - AssignmentId of assignment to be updated.
 * @param {Object} question - Question to be added into assignment.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.addQuestionToAssignment = async function(assignmentId, question, callback){
    if(!question){
        callback('Missing question', null)
    }

    try{
        if (question['answer'] == null || question['correctAnswer'] == null  || question['question'] == null || question['score'] == null ) {
            callback('Missing fields on question', null)
            return
        }
        const reply = await AssignmentQuestionCollection.add(question)
        const questionId = reply.id


        const assignment = await AssignmentCollection.doc(assignmentId).get();
        if(!assignment.exists){
            callback('Asssignment does not exist', null)
            return
        }

        const record = AssignmentCollection.doc(assignmentId);
        const unionRes = await record.update({
            questions: admin.firestore.FieldValue.arrayUnion(questionId)
        })
        callback(null, "Update successfully");

    } catch(err) {
        callback(err, null)
    }
}

/**
 * Remove assignment question from assignment by assignmentId.
 * @param {String} assignmentId - AssignmentId of assignment to be updated.
 * @param {String} assignmentQuestionId - AssignmentQuestionId of assignment question to be removed from assignment.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.removeQuestionFromAssignment = async function(assignmentId, assignmentQuestionId, callback){
    if(!assignmentQuestionId){
        callback('Missing assignmentQuestionId', null)
    }

    try{
        const assignment = await AssignmentCollection.doc(assignmentId).get();
        if(!assignment.exists){
            callback('Asssignment does not exist', null)
            return
        }
        const assignemntData = assignment.data();

        let found = false;
        for(let i = 0; i < assignemntData['questions'].length; i++){
            if(assignemntData['questions'][i] == assignmentQuestionId) found = true;
        }
        if(!found){
            callback('Asssignment question does not exist', null)
            return
        }
         
        await AssignmentQuestionCollection.doc(assignmentQuestionId).delete();
        const record = AssignmentCollection.doc(assignmentId);
        const removeRes = await record.update({
            questions: admin.firestore.FieldValue.arrayRemove(assignmentQuestionId)
        })
        callback(null, "Update successfully");

    } catch(err) {
        callback(err, null)
    }
}

/**
 * Delete an assignment from database by assignmentId.
 * @param {String} assignmentId - AssignmentId of assignment to be deleted.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.deleteAssignment = async function(assignmentId, callback){
    try{
        const res = await AssignmentCollection.doc(assignmentId).delete();
        callback(null, "Delete successfully")
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Get assignment data by assignmentId.
 * @param {String} assignmentId - AssignmentId of assignment.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.getAssignment = async function(assignmentId, callback){
    try{
        const assignment = await AssignmentCollection.doc(assignmentId).get();
        
        if(!assignment.exists){
            callback('Asssignment does not exist', null)
        }
        else{
            const data = assignment.data();
            data['assignmentId'] = assignmentId;
            data['startDate'] = dateToObject(data['startDate'].toDate())
            data['dueDate'] = dateToObject(data['dueDate'].toDate())
            callback(null, data);
        }
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Get all valid/opening assignment of student by matricNo of the student.
 * @param {String} matricNo - MatricNo number of student.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.getAssignmentsByMatricNo = async function(matricNo, callback){
    try{
        let result = await UsersCollection.where("matricNo", "==", matricNo).get();
        if (result.empty) {
            callback("User does not exists", null)
            return
        }

        const now = new Date();
        result = await AssignmentCollection.where('dueDate', '>=', now).get();
        

        let assignmentsData = {}
        result.forEach((doc) => {
            if(doc.data()['startDate'].toDate() <= now) assignmentsData[doc.id] = {"assignmentId":doc.id, "score": 0, ...doc.data()}
        });

        result = await AssignmentResultCollection.where("matricNo","==", matricNo).get();
        result.forEach((doc) => {
            let assignmentResult = doc.data();
            if(assignmentsData[assignmentResult['assignmentId']]){
                assignmentsData[assignmentResult['assignmentId']]['tries'] -= assignmentResult['tried'];
                assignmentsData[assignmentResult['assignmentId']]['score'] = assignmentResult['score'];
            }
            
        })
        
        const assignments = []
        for (const key in assignmentsData) {
            let data = {assignmentId: key, ...assignmentsData[key]};
            data['startDate'] = dateToString(data['startDate'].toDate())
            data['dueDate'] =  dateToString(data['dueDate'].toDate())
            assignments.push(data);
        }

        assignments.sort((a, b) => a.assignmentName < b.assignmentName ? -1 : (a.assignmentName > b.assignmentName ? 1 : 0));
        
        callback(null, assignments)
        
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Get all assignment data in the database.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.getAllAssignments = async function(callback){
    try{
        const snapshot = await AssignmentCollection.get();
       
        const assignments = []
        snapshot.forEach(doc =>{
        
            const data = doc.data();
            data['assignmentId'] = doc.id;
            data['startDate'] = dateToObject(data['startDate'].toDate())
            data['dueDate'] = dateToObject(data['dueDate'].toDate())
            assignments.push(data);
        });

        callback(null, assignments);
        
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Convert the date to a formatted object.
 * @param {Date} date - Date to be converted.
 * @returns {Object} result - Object contains year, month, day, hour and minute.
 */
function dateToObject(date){
    return{
        year: date.getFullYear(),
        month: date.getMonth()+1,
        day: date.getDate(),
        hour: date.getHours(),
        minute: date.getMinutes()
    }
}

/**
 * Convert the date to a formatted string.
 * @param {Date} date - Date to be converted.
 * @returns {String} result - Formatted string of date.
 */
function dateToString(m){
    return m.getFullYear()+ "/" +
        ("0" + (m.getMonth()+1)).slice(-2) + "/" +
        ("0" + m.getDate()).slice(-2) + " " +
        ("0" + m.getHours()).slice(-2) + ":" +
        ("0" + m.getMinutes()).slice(-2)
    
}