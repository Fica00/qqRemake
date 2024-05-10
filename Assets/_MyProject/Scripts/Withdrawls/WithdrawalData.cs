using System;

//each withdrawal has unique id
[Serializable]
public class WithdrawalData
{
    public string WithdrawalId;
    public string WalletAddress;
    public double Amount;
    public WithdrawalStatus Status;
    public string UserId;
    public string Error;
    public DateTime RequestTime;
    public DateTime ResolveTime;
}

//max and min amount to withdrawal
//add new field to each user DidRequestUserWallet, by default the value is on false, if it changes to true the server will passing new wallet address to that user
//the wallet will be stored in field userWallet, don't allow any wallet based actions before we receive the wallet address from user