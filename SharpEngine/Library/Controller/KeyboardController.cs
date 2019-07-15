using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;

namespace SharpEngine.Library.Controller
{
	public class KeyboardController : IController
	{
		private static bool[] _keys;
		private static int[] _mappings;
		public bool Get(Input type)
		{
			int code = _mappings[(int)type];
			return _keys[code];
		}

		public float GetValue(Input type)
		{
			float nVal = 0.0f;
			if(Get(type))
			{
				nVal = 1.0f;
			}

			return nVal;
		}

		private static KeyboardController _instance;
		public static KeyboardController Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new KeyboardController();
				}

				return _instance;
			}
		}


		private KeyboardController()
		{
			_keys = Enumerable.Repeat<bool>(false, 256).ToArray();
			_mappings = Enumerable.Repeat<int>(0, (int)Input.Error).ToArray();
			DefaultKeyMapping();
			_hookId = HookKeyboard(KeyEvent);
		}

		private static IntPtr HookKeyboard(Keyboard handler)
		{
			using (Process proc = Process.GetCurrentProcess())
			{
				using (ProcessModule module = proc.MainModule)
				{
					return SetWindowsHookEx(WH_KEYBOARD, handler, GetModuleHandle(module.ModuleName), 0);
				}
			}
		}

		public void Dispose()
		{
			if (_hookId != IntPtr.Zero)
			{
				UnhookWindowsHookEx(_hookId);
			}
		}

		public void DefaultKeyMapping()
		{
			Map(Input.Quit, Keys.Escape);
			Map(Input.Pause, Keys.P);
			Map(Input.Left, Keys.A);
			Map(Input.Right, Keys.D);
			Map(Input.Up, Keys.W);
			Map(Input.Down, Keys.S);
			Map(Input.Jump, Keys.Space);
			Map(Input.Fire, Keys.Space);
			/*
				Quit,
				Start,
				Stop,
				Pause,
				Left,
				Right,
				Up,
				Down,
				Jump,
				Jump2,
				Fire,
				Fire2,
				Error
			*/
		}

		public void Map(Input type, Keys key)
		{
			_mappings[(int)type] = (int)key;
		}

		private static IntPtr OnKeyHandler(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				int vkCode = Marshal.ReadInt32(lParam);
				if (wParam == (IntPtr)WM_KEYDOWN)
				{
					_keys[vkCode] = true;
				}
				else if(wParam == (IntPtr)WM_KEYUP)
				{
					_keys[vkCode] = false;
				}
			}
			else
			{
				String msg = String.Format("{0}", wParam);
			}
			
			return CallNextHookEx(_hookId, nCode, wParam, lParam);
		}

		#region Lowlevel Input

		private const int WH_KEYBOARD = 13;
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;
		private static Keyboard KeyEvent = OnKeyHandler;
		private static IntPtr _hookId;

		private delegate IntPtr Keyboard(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, Keyboard lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);
		#endregion
	}
}
