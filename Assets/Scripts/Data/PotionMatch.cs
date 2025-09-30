using System.Collections.Generic;

public class PotionMatch
{
    public EActionType ActionType;
    public (int w, int h) SourceIndex;
    public HashSet<(int w, int h)> TargetsIndex;
}