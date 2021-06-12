using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
    /*public enum BufferType
    {
	CameraColor,
	Custom
    }*/

    public class VolumetricFogFullscreenFeature : ScriptableRendererFeature
    {
	class RenderPass : ScriptableRenderPass
	{
	    private Material m_materialApplyFog;
	    private Material m_materialBlurr;
	    private RenderTargetIdentifier m_source;
	    private RenderTargetHandle m_tempTexture;
	    private int m_passIndex;

	    public RenderPass(Material _materialBlurr, Material _materialApplyFog, int _passIndex) : base()
	    {
		this.m_materialBlurr = _materialBlurr;
		this.m_materialApplyFog = _materialApplyFog;
		this.m_passIndex = _passIndex;
		m_tempTexture.Init("_TempBlurredVolumetric");
	    }

	    public void SetSource(RenderTargetIdentifier _source)
	    {
		this.m_source = _source;
	    }

	    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	    {
		CommandBuffer cmd = CommandBufferPool.Get("VolumetricBufferred");

		RenderTextureDescriptor cameraTextureDesc = renderingData.cameraData.cameraTargetDescriptor;
		cameraTextureDesc.depthBufferBits = 0;
		cmd.GetTemporaryRT(m_tempTexture.id, cameraTextureDesc, FilterMode.Bilinear);

		Blit(cmd, m_source, m_tempTexture.Identifier(), m_materialBlurr, m_passIndex);
		Blit(cmd, m_tempTexture.Identifier(), m_source, m_materialBlurr, m_passIndex);
		Blit(cmd, m_source, m_tempTexture.Identifier());
		Blit(cmd, m_tempTexture.Identifier(), m_source, m_materialApplyFog);

		context.ExecuteCommandBuffer(cmd);
		CommandBufferPool.Release(cmd);
	    }

	    public override void FrameCleanup(CommandBuffer cmd)
	    {
		cmd.ReleaseTemporaryRT(m_tempTexture.id);
	    }
	}

	[System.Serializable]
	public class Settings
	{
	    public Material materialBlurr;
	    public Material materialApplyFog;
	    public int materialPassIndex = -1; // all passes
	    public RenderPassEvent renderEvent = RenderPassEvent.BeforeRenderingPostProcessing;
	}

	[SerializeField]
	private Settings settings = new Settings();
	public Material MaterialBlurr
	{
	    get => settings.materialBlurr;
	}
	public Material MaterialApplyFog
	{
	    get => settings.materialApplyFog;
	}

	private RenderPass m_renderPass;

	public override void Create()
	{
	    this.m_renderPass = new RenderPass(settings.materialBlurr, settings.materialApplyFog, settings.materialPassIndex);

	    m_renderPass.renderPassEvent = settings.renderEvent;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	    m_renderPass.SetSource(renderer.cameraColorTarget);
	    renderer.EnqueuePass(m_renderPass);
	}
    }
}