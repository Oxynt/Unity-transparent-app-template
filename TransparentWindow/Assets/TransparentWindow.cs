using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

	private struct MARGINS
	{
		public int cxLeftWidth;
		public int cxRightWidth;
		public int cyTopHeight;
		public int cyBottomHeight;
	}
	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();
	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
	[DllImport("user32.dll")]
	static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
	[DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
	static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	private static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
	[DllImport("Dwmapi.dll")]
	private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
	const int GWL_STYLE = -16;
	const uint WS_POPUP = 0x80000000;
	const uint WS_VISIBLE = 0x10000000;
	//set window always at top
	const int HWND_TOPMOST = -1;

	public IntPtr hwnd;

	void Start()
	{
		#if !UNITY_EDITOR   // You really don't want to enable this in the editor..

		int fWidth = Screen.width;
		int fHeight = Screen.height;
		var margins = new MARGINS() { cxLeftWidth = -1 };
		hwnd = GetActiveWindow();


		// Window native UI

		//SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
		SetWindowLong(hwnd, GWL_STYLE, WS_POPUP);
		// Transparent windows with click through

		//declare layered window (shape or animated style)
		SetWindowLong(hwnd, -20, 524288 | 32);
		//GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L

		//opacity
		SetLayeredWindowAttributes(hwnd, 0, 255, 2);
		// Transparency=51=20%, LWA_ALPHA=2

		//keep window at top
		SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64);
		//SWP_FRAMECHANGED = 0x0020 (32);
		//SWP_SHOWWINDOW = 0x0040 (64);

		//always show window and full screen
		ShowWindowAsync(hwnd, 3);
	
		DwmExtendFrameIntoClientArea(hwnd, ref margins);
#endif
    }


    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }

//raycast to switch click trough
	Ray ray;
	RaycastHit hit;

	void Update()
	{

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		EventSystem eventSystem = EventSystem.current;

		if(Physics.Raycast(ray, out hit) || eventSystem.IsPointerOverGameObject())
		{
			#if !UNITY_EDITOR 
			SetWindowLong(hwnd, -20, 524288);
			#endif
			print ("hi");
		} else {
			#if !UNITY_EDITOR 
			SetWindowLong(hwnd, -20, 524288 | 32);
			#endif
		}



	}

}