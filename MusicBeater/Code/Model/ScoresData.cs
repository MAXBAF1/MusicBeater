using System;

namespace MusicBeater.Code.Model;

public class ScoresData
{
    public int IdealNiceCLickCnt;
    public int NiceCLickCnt;
    public int BadCLickCnt;
    private int TotalClickCnt => NiceCLickCnt - BadCLickCnt;
    public double GetPercentage() {
        var percentage = Math.Round(IdealNiceCLickCnt == 0 ? 0 : (double)TotalClickCnt / IdealNiceCLickCnt * 100);
        percentage = percentage switch
        {
            < -100 => -100,
            > 100 => 100,
            _ => percentage
        };

        return percentage;
    }
}