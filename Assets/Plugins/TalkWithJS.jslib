mergeInto(LibraryManager.library,
{
	AuthWithGoogle: function()
    {
		GoogleAuth();	
	},

	AuthWithTwitter: function()
	{
		TwitterAuth();
	},
	
    AuthWithDiscord: function()
    {
    	DiscordAuth();
    },

    AuthLinkingAnonimousUser: function(providerName)
    {
        LinkingAnonimousUser(UTF8ToString(providerName));
    },

	ShowKeyboard: function()
    {
		DisplayKeyboard();
    },

	CloseKeyboard: function()
    {
        HideKeyboard();
    },	

	OpenURL: function (url) 
	{
        window.location.href = UTF8ToString(url);
	},

    StripePurchaseInit: function(cost)
	{
	 	StripePurchase(cost)
	},
	
	DoSetUserId: function(id)
    {
        SetUserId(UTF8ToString(id));
    },
    
    IsPwa: function()
    {
       return CheckIsPwa();
    },
    
    DoCheckIfUserIsLoggedIn: function()
    {
       CheckUserSession();
    },
    
    DoAnonymousAuth: function()
    {
        return SignInAnonymous();
    },
    
    DoReload: function()
    {
        ReloadPageFromUnity();
    },
    
    DoSignOut: function()
    {
        SignOut();
    },    
    
    DoIsAndroid: function()
    {
        IsAndroid();
    },    
    
    DoCheckHasBoundAccount: function()
    {
        CheckHasBoundAccount();
    },    
    
    CheckIsOnPc: function()
    {
        return IsOnPc();
    },
    
    DoSendMessage: function(roomNamePtr, messagePtr)
    {
        sendMessage(UTF8ToString(roomNamePtr), UTF8ToString(messagePtr));
    },
    
    DoLeaveMatch: function()
    {
        leaveMatch();
    },
    
    DoMatchMakeAsync: function()
    {
        matchMakeAsync();
    },
   
    DoCancelMatchMake: function()
    {
        cancelMatchMake();
    },
    
    DoCreateAndSetupConnection: function(token)
    {
        createAndSetupConnection(UTF8ToString(token));
    },
    
    DoJoinFriendlyMatch: function(roomName)
    {
        joinFriendlyMatch(UTF8ToString(roomName));
    },
    
    DoCopyToClipboard: function(textPtr) {
        var text = UTF8ToString(textPtr);
    
        navigator.clipboard.writeText(text).then(function() {
          console.log('Copying to clipboard was successful!');
        }, function(err) {
          console.error('Could not copy text to clipboard: ', err);
        });
      },
      
      DoTellDeviceId: function(deviceId)
      {
        var text = UTF8ToString(deviceId);
        SaveAgencyUniqueDevice(text);
      }
});