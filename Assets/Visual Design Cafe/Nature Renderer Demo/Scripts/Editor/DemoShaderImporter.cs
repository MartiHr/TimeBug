using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "shaderdemo")]
public class DemoShaderImporter : ScriptedImporter
{
    [InitializeOnLoadMethod]
    public static void RegisterRenderPipelineDependency()
    {
        AssetDatabase.RegisterCustomDependency("nature-renderer/render-pipeline", Hash128.Compute(GetRenderPipelineName()));
    }

    private static string GetRenderPipelineName()
    {
        var renderPipeline = QualitySettings.renderPipeline;
        if (renderPipeline == null)
            renderPipeline = GraphicsSettings.defaultRenderPipeline;

        return renderPipeline?.GetType().Name ?? "BuiltIn";
    }

    public LazyLoadReference<TextAsset> BuiltIn;
    public LazyLoadReference<TextAsset> Universal;
    public LazyLoadReference<TextAsset> HighDefinition;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        string renderPipelineName = GetRenderPipelineName();


        string guid;
        TextAsset builtInSource = null;
        if(BuiltIn.isSet && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(BuiltIn, out  guid, out long _))
        {
            ctx.DependsOnArtifact(new GUID(guid));
            builtInSource = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guid));
        }

        TextAsset universalSource = null;
        if(Universal.isSet && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Universal, out guid, out long _))
        {
            ctx.DependsOnArtifact(new GUID(guid));
            universalSource = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guid));
        }

        TextAsset hdSource = null;
        if(HighDefinition.isSet && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(HighDefinition, out guid, out long _))
        {
            ctx.DependsOnArtifact(new GUID(guid));
            hdSource = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guid));
        }

        string source;
        if (renderPipelineName == "UniversalRenderPipelineAsset")
        {
            source = universalSource != null ? universalSource.text : "";
        }
        else if (renderPipelineName == "HDRenderPipelineAsset")
        {
            source = hdSource != null ? hdSource.text : "";
        }
        else
        {
            source = builtInSource != null ? builtInSource.text : "";
        }

        var shader = ShaderUtil.CreateShaderAsset(source);
        ShaderUtil.RegisterShader(shader);

        ctx.DependsOnCustomDependency("nature-renderer/render-pipeline");
        ctx.AddObjectToAsset("main", shader);
        ctx.SetMainObject(shader);
    }
}