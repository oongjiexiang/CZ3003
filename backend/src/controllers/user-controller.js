const admin = require("firebase-admin");
const db = admin.firestore();
const userRef = db.collection('users');

module.exports['createUser'] = async function(record, callback){
    if (record["matricNo"] == null|| record["username"]== null || record['character']== null) {
        callback('Missing fields', null)
        return
    }
    if(!record['openChallengeRating']) delete record['openChallengeRating'];
    if(!record['tutorialGroup']) delete record['tutorialGroup']

    try {
        const matricNumber = record["matricNo"];
        const result = await userRef.where("matricNo", "==", matricNumber).get();
        if (result.empty) {
            // just create a new item with random id 
            userRef.doc().set(record);
            callback(null, "User created");
        }
        // assuming that we already have the user with such user name and matric number dont do anything
        else {
            callback(null, "User already exist");
        }

    } catch (err) {
      callback(err, null);
    }
}

module.exports['updateUser'] = async function (matricNo, updateMap, callback) {
    try {
        const result = await userRef.where("matricNo", "==", matricNo).get();
        if (result.empty) {
            callback("User does not exists!",null)
        }
        else {
            result.forEach((doc) => {
                userRef.doc(doc.id).update(updateMap);
                callback(null,doc.id)
         });
        }
    }
    catch (err) {
        callback(err, null);
    }
}

module.exports['getUser'] = async function (matricNo, callback) {
    try {
        const result = await userRef.where("matricNo", "==", matricNo).get();
        if (result.empty) {
            callback("User does not exists!",null)
        }
        else {
            result.forEach((doc) => {
                const user = doc.data();
                console.log(user);
                callback(null,user)
            })
        }
    }
    catch (err) {
        callback(err, null);
    }
}

module.exports['deleteUser'] = async function (matricNo, callback) {
    try {
        console.log(matricNo);
        const result = await userRef.where("matricNo", "==", matricNo).get();
        console.log(result);
        if (result.empty) {
            callback("User does not exists!",null)
        } else {
            result.forEach((doc) => {
                userRef.doc(doc.id).delete();
                callback(null, "deleted");
            });
        }
    }
    catch (err) {
        callback(err, null);
    }
}

module.exports['getAllUsers'] = async function (callback) {
    try {
        const result = await userRef.get();
        if (result.empty) {
            callback("User does not exists!",null)
        }
        else {
            const users = []
            result.forEach((doc) => {
                users.push(doc.data());
            })
            callback(null, users);
        }
    }
    catch (err) {
        callback(err, null);
    }
}