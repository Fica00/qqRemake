mergeInto(LibraryManager.library,
{
	AuthWithGoogle: function()
        {
		GoogleAuth();	
	},

	AuthWithFacebook: function()
	{
		FacebookAuth();
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
});