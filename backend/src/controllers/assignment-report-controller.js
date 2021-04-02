const admin = require('firebase-admin');
const db = admin.firestore();
const assignmentResultCollection = db.collection("assignmentResult")
const tutorialGroupCollection = db.collection("tutorialGroup")

module.exports['getAssignmentReport'] = async function(queryMap, callback) {
    
    try{
        const result = await assignmentResultCollection.get();
        let resultData = []
        result.forEach((doc) => resultData.push(doc.data()));
        let assignmentIds = new Set();

        if(queryMap['tutorialGroupId'] != null){
            const record = await tutorialGroupCollection.doc(queryMap['tutorialGroupId']).get();
            if (!record.exists) {
                callback("Tutorial group not exist", null);
                return;
            }
            else {
                const student = record.data()['student'];
                resultData = resultData.filter(res => ((res['matricNo'] != null) && student.includes(res['matricNo'])));
            }
        }
        if(queryMap['assignmentId'] != null){
            resultData = resultData.filter(res => res['assignmentId'] == queryMap['assignmentId']);
        }

        resultData.forEach(data => assignmentIds.add(data['assignmentId']));
        const report = []
        assignmentIds.forEach((assignmentId) => {
            const assignmentResult = {}
            const raw_data = []
            const scores = []
            for (const entry of resultData) {
                if (entry['assignmentId'] == assignmentId) {
                    raw_data.push(entry)
                    scores.push(entry['score'])
                }
            }
            scores.sort((a,b) => a-b)
            const med = (scores[(scores.length - 1) >> 1] + scores[scores.length >> 1]) / 2
            assignmentResult['assignmentId'] = assignmentId
            assignmentResult['data'] = {
                min: Math.min(...scores),
                max: Math.max(...scores),
                mean: scores.reduce((a,b) => a+b, 0) / scores.length,
                median: med,
                rawData: raw_data
            }
            report.push(assignmentResult)          
        })
        callback(null, report)
    } catch(err) {
        callback(err, null)
    }
}
