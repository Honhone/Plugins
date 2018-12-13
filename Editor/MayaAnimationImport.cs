using UnityEditor;

public class MayaAnimationImport : AssetPostprocessor
{
    private const string clipName = "Take 001";
    private const int scale = 100;
    private const bool isLoop = true;
    private const bool isPoseLoop = true;
    private ModelImporterAnimationCompression compression = ModelImporterAnimationCompression.Optimal;
    /// <summary>
    /// This preprocessor script automates some minor inconveniences importing animations into unity
    /// It will set up basic looping, rename take 001 into asset's name and fix scale.
    /// You can change settings above to tie into your preferred 3DDCC specifics. 
    /// Credits go to original author christian-schulze:
    /// http://answers.unity.com/answers/1521776/view.html
    /// </summary>

    void OnPreprocessAnimation()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        if (modelImporter.clipAnimations.Length > 0) //process only solo clips
        {
            return;
        }
        ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;
        if (clipAnimations.Length == 1 && clipAnimations[0] != null && clipAnimations[0].name.Contains(clipName))
        {
            modelImporter.globalScale = scale;
            modelImporter.animationCompression = compression;
            clipAnimations[0].name = GetAssetName();
            clipAnimations[0].loopTime = isLoop;
            clipAnimations[0].loopPose = isPoseLoop;            
            modelImporter.clipAnimations = clipAnimations;
        }
    }

    private string GetAssetName()
    {
        int begin = assetPath.LastIndexOf('/');
        if (begin < 0)
        {
            begin = 0;
        }
        int end = assetPath.LastIndexOf('.');
        if (end < 0)
        {
            end = assetPath.Length - 1;
        }
        return assetPath.Substring(begin + 1, end - begin - 1);
    }
}
