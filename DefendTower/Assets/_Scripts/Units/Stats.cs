/// <summary>
/// Data storage for tower stats
/// </summary>
public class Stats {
    public int BaseStr { get; private set; }
    public int BaseDex { get; private set; }
    public int BaseCon { get; private set; }
    public int BaseCha { get; private set; }
    public int BaseWin { get; private set; }
    public int BaseInt { get; private set; }

    public int TmpStr { get; private set; }
    public int TmpDex { get; private set; }
    public int TmpCon { get; private set; }
    public int TmpCha { get; private set; }
    public int TmpWis { get; private set; }
    public int TmpInt { get; private set; }

    public int Str => BaseStr + TmpStr;
    public int Dex => BaseDex + TmpDex;
    public int Con => BaseCon + TmpCon;
    public int Cha => BaseCha + TmpCha;
    public int Wis => BaseWin + TmpWis;
    public int Int => BaseInt + TmpInt;



    public Stats(int value, StatType statType) {

    }
    /// <summary>
    /// Change the stat of the tower
    /// </summary>
    /// <param name="value">How much the stat will be changed</param>
    /// <param name="statType">Which stat will be changed</param>
    /// <param name="time">For how long the stat will be increased. -1 for indefinitely</param>
    public void Change(int value, StatType statType, float time = -1) {

        switch(statType) {
            case StatType.Str:
                var stat = time > 0 ? BaseWin : TmpWis;
                break;
            case StatType.Dex:
                break;
            case StatType.Con:
                break;
            case StatType.Int:
                break;
            case StatType.Cha:
                break;
            case StatType.Wis:
                break;
        }
    }
}

