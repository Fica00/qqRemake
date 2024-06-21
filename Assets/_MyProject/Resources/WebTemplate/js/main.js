let currentUser = null;
let userId = null;
let unity = null;
let isBreakingScript = false;
let isEvAgency = false;
const btnCancelPayment = document.querySelector("#btn-cancelPayment");
const stripe = Stripe(
  "pk_live_51O9azOCJmlrAlGBoAojhLshRHV3rr5qqUXrdG13NYxbpkfLU69WVgypk06ZiARta7y7LSA3wEhbJNf2K6CvWP5Wf00trRX4rYy"
);
let dinamicParam = new URLSearchParams(window.location.search).get("a");
// Proveri da li "a" parametar postoji, ako ne postoji generiši "dinamicParam" iz sub1, sub2, sub3 i sub4
console.log(dinamicParam);
if (!dinamicParam) {
  const urlParams = new URLSearchParams(window.location.search);
  console.log(urlParams);
  const sub1 = urlParams.get("sub1");
  console.log(sub1);
  const sub2 = urlParams.get("sub2");
  console.log(sub2);
  const sub3 = urlParams.get("sub3");
  console.log(sub3);
  const sub4 = urlParams.get("sub4");
  console.log(sub4);
  const sub5 = urlParams.get("sub5");
  if (sub1 && sub2 && sub3 && sub4) {
    isEvAgency = true;
    dinamicParam = `${sub1}${sub2}${sub3}${sub4}${sub5}`;
  } else {
    dinamicParam = "All Users";
  }
}

document.addEventListener("unityInitialized", function () {
  CheckUserSession();
});

// Zamenjeno: Korišćenje Web Crypto API-ja umesto require("crypto")
async function generateSignature(clientId, clientSecret, body, timestamp) {
  const encoder = new TextEncoder();
  const keyData = encoder.encode(clientSecret);
  const cryptoKey = await crypto.subtle.importKey(
    "raw",
    keyData,
    { name: "HMAC", hash: "SHA-256" },
    false,
    ["sign"]
  );

  const message = `${clientId}${timestamp}${JSON.stringify(body)}`;
  const messageData = encoder.encode(message);
  const signatureArrayBuffer = await crypto.subtle.sign(
    "HMAC",
    cryptoKey,
    messageData
  );
  const signature = Array.from(new Uint8Array(signatureArrayBuffer))
    .map((b) => b.toString(16).padStart(2, "0"))
    .join("");
  return signature;
}

async function sendUserIdentifierToEarnAlliance(userId, email) {
  const clientId = "31941903-89d8-4bad-8e85-9b05754eecdb";
  const clientSecret = "pbrs3SRCUbO92iir14s8VnlBDA5uDQE9";
  const gameId = "91e6b829-3371-4edb-b45f-93bc1bc22063";
  const url = "https://events.earnalliance.com/v2/custom-events";

  const timestamp = Date.now();
  const body = {
    gameId: gameId,
    identifiers: [
      {
        userId: userId,
        email: email,
      },
    ],
  };

  const signature = await generateSignature(
    clientId,
    clientSecret,
    body,
    timestamp
  );

  try {
    const response = await axios.post(url, body, {
      headers: {
        "x-client-id": clientId,
        "x-timestamp": timestamp,
        "x-signature": signature,
        "Content-Type": "application/json",
      },
    });
    console.log("User identifier sent to EarnAlliance:", response.data);
  } catch (error) {
    console.error("Error sending user identifier to EarnAlliance:", error);
  }
}

async function sendEventToEarnAlliance(eventType, userId) {
  const clientId = "31941903-89d8-4bad-8e85-9b05754eecdb";
  const clientSecret = "pbrs3SRCUbO92iir14s8VnlBDA5uDQE9";
  const gameId = "91e6b829-3371-4edb-b45f-93bc1bc22063";
  const url = "https://events.earnalliance.com/v2/custom-events";

  const timestamp = Date.now();
  const body = {
    gameId: gameId,
    events: [
      {
        userId: userId,
        event: eventType,
        time: new Date().toISOString(),
      },
    ],
  };

  const signature = await generateSignature(
    clientId,
    clientSecret,
    body,
    timestamp
  );

  try {
    const response = await axios.post(url, body, {
      headers: {
        "x-client-id": clientId,
        "x-timestamp": timestamp,
        "x-signature": signature,
        "Content-Type": "application/json",
      },
    });
    console.log("Event sent to EarnAlliance:", response.data);
  } catch (error) {
    console.error("Error sending event to EarnAlliance:", error);
  }
}

function isEarnAllianceLink() {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get("test") === "earnalliance";
}

function CheckHasBoundAccount() {
  firebase.auth().onAuthStateChanged(function (user) {
    if (user) {
      let isBounded = !user.isAnonymous;
      unity.SendMessage(
        "JavaScriptManager",
        "HasBoundedAccount",
        JSON.stringify({ IsBounded: isBounded })
      );
    }
  });
}

function IsAndroid() {
  return /Android/.test(navigator.userAgent);
}

function CheckIsPwa() {
  function IsInStandaloneMode() {
    var displayModes = ["fullscreen", "standalone", "minimal-ui"];
    return displayModes.some(
      (displayMode) =>
        window.matchMedia("(display-mode: " + displayMode + ")").matches
    );
  }

  const isIOS = /iPhone|iPad|iPod/.test(navigator.userAgent);
  const isAndroid = /Android/.test(navigator.userAgent);
  const isOnPC = !isIOS && !isAndroid;

  let res;
  if (!isOnPC && IsInStandaloneMode()) {
    res = true; // is PWA
  } else {
    res = false; // is not PWA
  }

  return res;
}

function IsOnPc() {
  const isIOS = /iPhone|iPad|iPod/.test(navigator.userAgent);
  const isAndroid = /Android/.test(navigator.userAgent);
  const isOnPC = !isIOS && !isAndroid;

  return isOnPC;
}

function SetUserId(id) {
  userId = id;

  // Reference to the user's 'Coins' field in Firebase
  var coinsRef = firebase.database().ref(`users/${userId}/Coins`);
  // Listener for changes in the 'Coins' field
  coinsRef.on("value", function (snapshot) {
    if (snapshot.exists()) {
      const coins = snapshot.val();
      //unity.SendMessage('JavaScriptManager', 'OnUpdatedDiamonds', coins.toString());
    }
  });

  // Reference to the user's 'USDC' field in Firebase
  var usdcRef = firebase.database().ref(`users/${userId}/USDC`);
  // Listener for changes in the 'USDC' field
  usdcRef.on("value", function (snapshot) {
    if (snapshot.exists()) {
      const usdc = snapshot.val();
      unity.SendMessage("JavaScriptManager", "OnUpdatedUSDC", usdc.toString());
    }
  });
}

async function updateMarketingParam(param, userId) {
  if (!param) {
    console.log("No dynamicParam found in URL");
    return;
  }

  let _randomId = crypto.randomUUID();
  let countryName = null;
  let cityName = null;
  const _key = "uniqueIdForMarketing";
  let _idFromStorage = localStorage.getItem(_key);
  let _hadId = _idFromStorage != null;
  let _updateDevices = false;
  if (_hadId) {
    _randomId = _idFromStorage;
  } else {
    localStorage.setItem(_key, _randomId);
    _updateDevices = true;
  }

  console.log("Found agency: " + param);
  const agencyRefforIsEvAgency = firebase.database().ref(`marketing/${param}`);
  const agencyRef = firebase.database().ref(`marketing/${param}/click`);
  agencyRef.once("value", async function (snapshot) {
    console.log("Got value");

    try {
      const geoData = await getGeolocation();
      countryName = geoData.country_name;
      cityName = geoData.city;
      agencyRef.push(
        {
          timestamp: new Date().toISOString(),
          deviceId: _randomId,
          country: countryName,
          city: cityName,
        },
        function (error) {
          if (error) {
            console.error("Error updating marketing user:", error);
          } else {
            console.log(
              `User '${userId}' added to agency '${param}' with timestamp and country.`
            );
          }
        }
      );

      agencyRefforIsEvAgency.update(
        {
          isEvAgency: isEvAgency,
        },
        function (error) {
          if (error) {
            console.error("Error updating isEvAgency:", error);
          } else {
            console.log(
              `Agency '${param}' updated with isEvAgency: ${isEvAgency}.`
            );
          }
        }
      );

      if (!_updateDevices) {
        return;
      }

      await SaveAgencyUniqueDevice(_randomId, countryName, cityName);
    } catch (error) {
      console.error(
        "Failed to get geolocation data for marketing param:",
        error
      );
    }
  });
}

async function SaveAgencyUniqueDevice(deviceId, countryName, cityName) {
  if (
    dinamicParam === null ||
    dinamicParam === undefined ||
    dinamicParam === ""
  ) {
    return;
  }
  console.log("Got device id: " + deviceId);
  const agencyRef = firebase
    .database()
    .ref(`marketing/${dinamicParam}/devices/${deviceId}`);
  agencyRef.once("value", function (snapshot) {
    console.log("Got value");
    agencyRef.set(
      {
        timestamp: new Date().toISOString(),
        country: countryName,
        city: cityName,
      },
      function (error) {
        if (error) {
          console.error("Error updating marketing user:", error);
        } else {
          console.log(
            `User '${userId}' added to agency '${dinamicParam}' with timestamp.`
          );
        }
      }
    );
  });
}

updateMarketingParam(dinamicParam);

const ReloadPageFromUnity = () => {
  location.reload();
};

const CheckUserSession = () => {
  console.log("Checking session");
  if (!unity) {
    console.log("Unity instance is not ready.");
    return; // Exit the function if unity is not initialized
  }
  if (currentUser) {
    tellUnityUserInfo(currentUser.uid, currentUser.isAnonymous, currentUser);
  } else {
    unity.SendMessage("JavaScriptManager", "AuthFinished", "");
  }
};

function LinkingAnonimousUser(providerName) {
  var user = firebase.auth().currentUser;
  if (!user) return; //if user doesn't exsist that means that client is not logged in
  let provider = null;
  switch (
    providerName //choose a provider based on string that is provided by specific button from unity
  ) {
    case "google":
      provider = googleProvider;
      break;
    case "facebook":
      provider = facebookProvider;
      break;
    case "twitter":
      provider = twitterProvider;
      break;
    default:
      console.log("Unsupported provider");
      return;
  }
  user
    .linkWithPopup(provider)
    .then((result) => {
      //call sign up with specific provider
      var credential = result.credential;
      var user = result.user;
      unity.SendMessage("JavaScriptManager", "SuccessLinkingLoginAccount");
    })
    .catch((error) => {
      console.log(error);
      unity.SendMessage("JavaScriptManager", "UserAlreadyHasAccount");
    });
}

function tellUnityUserInfo(userId, isGuest, userInfo) {
  var ref = firebase.database().ref(`users/${userId}`);
  ref
    .once("value", function (snapshot) {
      if (snapshot.exists()) {
        const userData = snapshot.val();
        console.log("User Data Retrieved:", userData);

        unity.SendMessage(
          "JavaScriptManager",
          "AuthFinished",
          JSON.stringify({
            UserId: userId,
            IsNewAccount: false,
            Agency: dinamicParam,
            IsGuest: isGuest,
          })
        );
      } else {
        console.log("No user data available for user ID:", userId);
        console.log("Before calling conversion!!");

        // Dodato: Slanje identifikatora korisnika EarnAlliance API-ju
        if (isEarnAllianceLink()) {
          sendUserIdentifierToEarnAlliance(userId, userInfo.email);
        }

        // Dodato: Slanje događaja EarnAlliance API-ju
        if (isEarnAllianceLink()) {
          sendEventToEarnAlliance("USER_SIGNUP", userId);
        }

        triggerConversionScript();
        unity.SendMessage(
          "JavaScriptManager",
          "AuthFinished",
          JSON.stringify({
            UserId: userId,
            IsNewAccount: true,
            Agency: dinamicParam,
            IsGuest: isGuest,
          })
        );
      }
    })
    .catch(function (error) {
      console.error("Error fetching user data:", error);
      unity.SendMessage("JavaScriptManager", "FailedToAuth");
    });
}

function checkAndReauthenticateUser() {
  const userAuthDataString = localStorage.getItem("userReauthenticate");
  if (userAuthDataString) {
    const userAuthData = JSON.parse(userAuthDataString);
    const { uid, token, provider } = userAuthData;

    let credential;
    if (provider === "google") {
      credential = firebase.auth.GoogleAuthProvider.credential(null, token);
    } else if (provider === "twitter") {
      credential = firebase.auth.TwitterAuthProvider.credential(token);
    }

    if (credential) {
      firebase
        .auth()
        .signInWithCredential(credential)
        .then((userCredential) => {
          console.log("User re-authenticated:", userCredential);

          tellUnityUserInfo(
            userCredential.user.uid,
            userCredential.user.isAnonymous,
            userCredential.user
          );
        })
        .catch((error) => {
          console.error("Re-authentication failed:", error);
          localStorage.removeItem("userReauthenticate");
        });
    }
  }
}

checkAndReauthenticateUser();

const SignOut = () => {
  firebase
    .auth()
    .signOut()
    .then(() => {
      localStorage.removeItem("userReauthenticate");
      ReloadPageFromUnity();
    })
    .catch((error) => {
      console.log("greska");
    });
};
