/**
 * Controller for open challenge record related logic.
 * @module open-challenge-record-controller
 * @category controller
 */

const admin = require('firebase-admin');
const db = admin.firestore();
const openChallengeRecordCollection = db.collection("openChallengeRecord")

/**
 * Create open challenge record and store into the database. Open challenge record must have necessary field.
 * @param {Object} record - New open challenge record details, including question, team1, team2 and type.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.createOpenChallengeRecord = async function(record, callback) {
    if (record['questions'] == null || record['team1'] == null || record['team2'] == null || record['type'] == null) {
        callback('Missing fields', null)
        return
    }
    try{
        const reply = await openChallengeRecordCollection.add(record)
        callback(null, reply.id)
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Get all open challenge records data.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.getAllOpenChallengeRecords = async function(callback) {
    try{ 
        const snapshot = await openChallengeRecordCollection.get()
        if (snapshot.empty) {
            callback('No data', null)
        }
        else {
            var res = []
            snapshot.forEach(doc => {
                const record = doc.data();
                record.openChallengeRecordId = doc.id
                res.push(record)
            })
            callback(null, res)
        }
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Get open challenge record data by recordId.
 * @param {String} recordId - recordId of open challenge record.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.getOpenChallengeRecord = async function(recordId, callback) {
    try{ 
        const record = await openChallengeRecordCollection.doc(recordId).get()
        if (!record.exists) {
            callback('No such record found', null)
        }
        else {
            const data = record.data();
            data.openChallengeRecordId = recordId
            callback(null, data)
        }
    } catch(err) {
        callback(err, null)
    }
}


/**
 * Update open challenge record by recordId, updated field name must be valid.
 * @param {String} recordId - RecordId of open challenge record to be updated.
 * @param {Object} updateFields - Object include new data to update.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.updateOpenChallengeRecord = async function(recordId, updateFields, callback) {
    try{
        const record = await openChallengeRecordCollection.doc(recordId).get()
        if (!record.exists) {
            callback('No such record found', null)
        }
        else{
            const res = await openChallengeRecordCollection.doc(recordId).update(updateFields)
            callback(null, "Update successfully")
        }
    } catch(err) {
        callback(err, null)
    }
}

/**
 * Delete open challenge record by recordId.
 * @param {String} recordId - RecordId of open challenge record to be updated.
 * @param {requestCallback} callback - A callback to return http response.
 */
module.exports.deleteOpenChallengeRecord = async function(recordId, callback) {
    try{
        const res = await openChallengeRecordCollection.doc(recordId).delete()
        callback(null, "Delete successfully")
        
    } catch(err) {
        callback(err, null)
    }
}
