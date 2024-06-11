const googleProvider = new firebase.auth.GoogleAuthProvider()
const twitterProvider = new firebase.auth.TwitterAuthProvider()
const facebookProvider = new firebase.auth.FacebookAuthProvider()

async function getIPAddress() {
  try {
    const response = await axios.get("https://api.ipify.org?format=json")
    return response.data.ip
  } catch (error) {
    console.error("Failed to get IP address:", error)
    throw error
  }
}
async function getGeolocation() {
  try {
    const ip = await getIPAddress()
    const response = await axios.get(`https://ipapi.co/${ip}/json/`)
    return response.data
  } catch (error) {
    console.error("Failed to get geolocation data:", error)
    throw error
  }
}

function GoogleAuth() {
  firebase
    .auth()
    .signInWithPopup(googleProvider)
    .then((result) => {
      console.log("Google auth: " + result)

      const userAuthData = {
        uid: result.user.uid,
        token: result.credential.accessToken,
        provider: "google",
      }

      localStorage.setItem("userReauthenticate", JSON.stringify(userAuthData))

      tellUnityUserInfo(result.user.uid, result.user.isAnonymous, result.user)
    })
    .catch((err) => {
      unity.SendMessage("JavaScriptManager", "FailedToAuth")
      console.log(err.code)
    })
}

function TwitterAuth() {
  firebase
    .auth()
    .signInWithPopup(twitterProvider)
    .then((result) => {
      console.log("Twitter auth: " + result)

      const userAuthData = {
        uid: result.user.uid,
        token: result.credential.accessToken,
        provider: "twitter",
      }

      localStorage.setItem("userReauthenticate", JSON.stringify(userAuthData))

      tellUnityUserInfo(result.user.uid, result.user.isAnonymous, result.user)
    })
    .catch((err) => {
      unity.SendMessage("JavaScriptManager", "FailedToAuth")
      console.log(err.code)
    })
}

function SignInAnonymous() {
  firebase
    .auth()
    .signInAnonymously()
    .then((userCredential) => {
      console.log("Anonymous auth: " + userCredential)

      const userAuthData = {
        uid: userCredential.user.uid,
        token: userCredential.user.stsTokenManager.accessToken,
        provider: "anonymous",
      }

      localStorage.setItem("userReauthenticate", JSON.stringify(userAuthData))

      tellUnityUserInfo(
        userCredential.user.uid,
        userCredential.user.isAnonymous,
        userCredential.user
      )
    })
    .catch((error) => {
      unity.SendMessage("JavaScriptManager", "FailedToAuth")
      console.log(error.code)
    })
}
