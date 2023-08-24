mergeInto(LibraryManager.library,
{
	AuthWithGoogle: function()
        {
		GoogleAuth();	
	},

	AuthWithFacebook: function()
	{
		FacebookAuth();
	}
});