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
});