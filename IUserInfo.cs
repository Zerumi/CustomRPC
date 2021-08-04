namespace CustomRPC
{
    public interface IUserInfo
    {
        int LangIndex { get; }

        string ClientError();

        string ClientDisconnected();

        string LatestStatusUpdate();

        string TimeNull();

        string Exit();

        void MBmiGameNameHelp();

        void MBmiPSize();

        void MBmiImgChange();

        void MBmiTimestFormat();

        string UImRPC();

        string UImRPCappID();

        string UImRPClargeIMG();

        string UImRPCsmallIMG();

        string UImRPCtimestamp();

        string UImRPCpartyID();

        string UImRPCspecCD();

        string UImRPCask2joinCD();

        string UImConfig();

        string UImcSave();

        string UImcLoad();

        string UImHelp();

        string UImGameNameHelp();

        string UImPSize();

        string UImImgChange();

        string UImTimestFormat();

        string UIbApply();

        string UIhlDDP();

        string UIhlLinktoapp();

        string UImLang();
    }
}
