﻿/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tizen.NUI.BaseComponents;
using System.Collections.Generic;

namespace Tizen.NUI
{
    /// <summary>
    /// The window class is used internally for drawing.<br />
    /// The window has an orientation and indicator properties.<br />
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public partial class Window
    {
        private WindowFocusChangedEventCallbackType windowFocusChangedEventCallback;
        private RootLayerTouchDataCallbackType rootLayerTouchDataCallback;
        private RootLayerTouchDataCallbackType rootLayerInterceptTouchDataCallback;
        private WheelEventCallbackType wheelEventCallback;
        private EventCallbackDelegateType1 stageKeyCallbackDelegate;
        private InterceptKeyEventDelegateType stageInterceptKeyCallbackDelegate;
        private EventCallbackDelegateType0 stageEventProcessingFinishedEventCallbackDelegate;
        private EventHandler stageContextLostEventHandler;
        private EventCallbackDelegateType0 stageContextLostEventCallbackDelegate;
        private EventHandler stageContextRegainedEventHandler;
        private EventCallbackDelegateType0 stageContextRegainedEventCallbackDelegate;
        private EventHandler stageSceneCreatedEventHandler;
        private EventCallbackDelegateType0 stageSceneCreatedEventCallbackDelegate;
        private WindowResizeEventCallbackType windowResizeEventCallback;
        private WindowFocusChangedEventCallbackType windowFocusChangedEventCallback2;
        private TransitionEffectEventCallbackType transitionEffectEventCallback;
        private MovedEventCallbackType movedEventCallback;
        private OrientationChangedEventCallbackType orientationChangedEventCallback;
        private KeyboardRepeatSettingsChangedEventCallbackType keyboardRepeatSettingsChangedEventCallback;
        private AuxiliaryMessageEventCallbackType auxiliaryMessageEventCallback;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void WindowFocusChangedEventCallbackType(IntPtr window, bool focusGained);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool RootLayerTouchDataCallbackType(IntPtr view, IntPtr touchData);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool WheelEventCallbackType(IntPtr view, IntPtr wheelEvent);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void WindowResizeEventCallbackType(IntPtr window, IntPtr windowSize);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void WindowFocusChangedEventCallbackType2(IntPtr window, bool focusGained);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void TransitionEffectEventCallbackType(IntPtr window, int state, int type);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void MovedEventCallbackType(IntPtr window, IntPtr position);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OrientationChangedEventCallbackType(IntPtr window, int orientation);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void KeyboardRepeatSettingsChangedEventCallbackType();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void AuxiliaryMessageEventCallbackType(IntPtr kData, IntPtr vData, IntPtr optionsArray);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool InterceptKeyEventDelegateType(IntPtr arg1);

        /// <summary>
        /// FocusChanged event.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<FocusChangedEventArgs> FocusChanged
        {
            add
            {
                if (windowFocusChangedEventHandler == null)
                {
                    windowFocusChangedEventCallback = OnWindowFocusedChanged;
                    using WindowFocusSignalType signal = new WindowFocusSignalType(Interop.Window.FocusChangedSignal(SwigCPtr), false);
                    signal?.Connect(windowFocusChangedEventCallback);
                }
                windowFocusChangedEventHandler += value;
            }
            remove
            {
                windowFocusChangedEventHandler -= value;
                if (windowFocusChangedEventHandler == null && windowFocusChangedEventCallback != null)
                {
                    using WindowFocusSignalType signal = new WindowFocusSignalType(Interop.Window.FocusChangedSignal(SwigCPtr), false);
                    signal?.Disconnect(windowFocusChangedEventCallback);
                    if (signal?.Empty() == true)
                    {
                        windowFocusChangedEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// Emits the event when the screen is touched and when the touch ends.<br />
        /// If there are multiple touch points then it is emitted when the first touch occurs and
        /// when the last finger is lifted too.<br />
        /// Even though incoming events are interrupted, the event occurs.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<TouchEventArgs> TouchEvent
        {
            add
            {
                if (rootLayerTouchDataEventHandler == null)
                {
                    rootLayerTouchDataCallback = OnWindowTouch;
                    using TouchDataSignal signal = new TouchDataSignal(Interop.ActorSignal.ActorTouchSignal(Layer.getCPtr(GetRootLayer())), false);
                    signal?.Connect(rootLayerTouchDataCallback);
                }
                rootLayerTouchDataEventHandler += value;
            }
            remove
            {
                rootLayerTouchDataEventHandler -= value;
                if (rootLayerTouchDataEventHandler == null && rootLayerTouchDataCallback != null)
                {
                    using TouchDataSignal signal = new TouchDataSignal(Interop.ActorSignal.ActorTouchSignal(Layer.getCPtr(GetRootLayer())), false);
                    signal?.Disconnect(rootLayerTouchDataCallback);
                    if (signal?.Empty() == true)
                    {
                        rootLayerTouchDataCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// An event for the touched signal which can be used to subscribe or unsubscribe the event handler provided by the user.<br />
        /// The touched signal is emitted when the touch input is received.<br />
        /// This can receive touch events before child. <br />
        /// If it returns false, the child can receive the touch event. If it returns true, the touch event is intercepted. So child cannot receive touch event.<br />
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event ReturnTypeEventHandler<object, TouchEventArgs, bool> InterceptTouchEvent
        {
            add
            {
                if (rootLayerInterceptTouchDataEventHandler == null)
                {
                    rootLayerInterceptTouchDataCallback = OnWindowInterceptTouch;
                    using TouchDataSignal signal = new TouchDataSignal(Interop.ActorSignal.ActorInterceptTouchSignal(Layer.getCPtr(GetRootLayer())), false);
                    signal?.Connect(rootLayerInterceptTouchDataCallback);
                }
                rootLayerInterceptTouchDataEventHandler += value;
            }
            remove
            {
                rootLayerInterceptTouchDataEventHandler -= value;
                if (rootLayerInterceptTouchDataEventHandler == null && rootLayerInterceptTouchDataCallback != null)
                {
                    using TouchDataSignal signal = new TouchDataSignal(Interop.ActorSignal.ActorInterceptTouchSignal(Layer.getCPtr(GetRootLayer())), false);
                    signal?.Disconnect(rootLayerInterceptTouchDataCallback);
                    if (signal?.Empty() == true)
                    {
                        rootLayerInterceptTouchDataCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// Emits the event when the wheel event is received.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<WheelEventArgs> WheelEvent
        {
            add
            {
                if (stageWheelHandler == null)
                {
                    wheelEventCallback = OnStageWheel;
                    using WheelSignal signal = new WheelSignal(Interop.ActorSignal.ActorWheelEventSignal(Layer.getCPtr(this.GetRootLayer())), false);
                    signal?.Connect(wheelEventCallback);
                }
                stageWheelHandler += value;

                if (DetentEventHandler == null)
                {
                    DetentEventCallback = OnDetentEvent;
                    using StageWheelSignal signal = new StageWheelSignal(Interop.StageSignal.WheelEventSignal(stageCPtr), false);
                    signal?.Connect(DetentEventCallback);
                }
                DetentEventHandler += value;
            }
            remove
            {
                stageWheelHandler -= value;
                if (stageWheelHandler == null && wheelEventCallback != null)
                {
                    using WheelSignal signal = new WheelSignal(Interop.ActorSignal.ActorWheelEventSignal(Layer.getCPtr(this.GetRootLayer())), false);
                    signal?.Disconnect(wheelEventCallback);
                    if (signal?.Empty() == true)
                    {
                        wheelEventCallback = null;
                    }
                }

                DetentEventHandler -= value;
                if (DetentEventHandler == null && DetentEventCallback != null)
                {
                    using StageWheelSignal signal = new StageWheelSignal(Interop.StageSignal.WheelEventSignal(stageCPtr), false);
                    signal?.Disconnect(DetentEventCallback);
                    if (signal?.Empty() == true)
                    {
                        DetentEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// Emits the event when the key event is received.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<KeyEventArgs> KeyEvent
        {
            add
            {
                if (stageKeyHandler == null)
                {
                    stageKeyCallbackDelegate = OnStageKey;
                    using KeyEventSignal signal = new KeyEventSignal(Interop.Window.KeyEventSignal(SwigCPtr), false);
                    signal?.Connect(stageKeyCallbackDelegate);
                }
                stageKeyHandler += value;
            }
            remove
            {
                stageKeyHandler -= value;
                if (stageKeyHandler == null && stageKeyCallbackDelegate != null)
                {
                    using KeyEventSignal signal = new KeyEventSignal(Interop.Window.KeyEventSignal(SwigCPtr), false);
                    signal?.Disconnect(stageKeyCallbackDelegate);
                    if (signal?.Empty() == true)
                    {
                        stageKeyCallbackDelegate = null;
                    }
                }
            }
        }

        /// <summary>
        /// Intercepts KeyEvents in the window before dispatching KeyEvents to the child.<br />
        /// If it returns true(consumed), no KeyEvent is delivered to the child.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event ReturnTypeEventHandler<object, KeyEventArgs, bool> InterceptKeyEvent
        {
            add
            {
                if (stageInterceptKeyHandler == null)
                {
                    stageInterceptKeyCallbackDelegate = OnStageInterceptKey;
                    using KeyEventSignal signal = new KeyEventSignal(Interop.Window.InterceptKeyEventSignal(SwigCPtr), false);
                    signal?.Connect(stageInterceptKeyCallbackDelegate);
                }
                stageInterceptKeyHandler += value;
            }
            remove
            {
                stageInterceptKeyHandler -= value;
                if (stageInterceptKeyHandler == null && stageInterceptKeyCallbackDelegate != null)
                {
                    using KeyEventSignal signal = new KeyEventSignal(Interop.Window.InterceptKeyEventSignal(SwigCPtr), false);
                    signal?.Disconnect(stageInterceptKeyCallbackDelegate);
                    if (signal?.Empty() == true)
                    {
                        stageInterceptKeyCallbackDelegate = null;
                    }
                }
            }
        }

        /// <summary>
        /// Emits the event when the window resized.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<ResizedEventArgs> Resized
        {
            add
            {
                if (windowResizeEventHandler == null)
                {
                    windowResizeEventCallback = OnResized;
                    using ResizeSignal signal = new ResizeSignal(Interop.Window.ResizeSignal(SwigCPtr), false);
                    signal?.Connect(windowResizeEventCallback);
                }

                windowResizeEventHandler += value;
            }
            remove
            {
                windowResizeEventHandler -= value;
                if (windowResizeEventHandler == null && windowResizeEventCallback != null)
                {
                    using ResizeSignal signal = new ResizeSignal(Interop.Window.ResizeSignal(SwigCPtr), false);
                    signal?.Disconnect(windowResizeEventCallback);
                    if (signal?.Empty() == true)
                    {
                        windowResizeEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// Do not use this, that will be deprecated. Use 'FocusChanged' event instead.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// Do not use this, that will be deprecated.
        /// Instead Use FocusChanged.
        [Obsolete("Do not use this, that will be deprecated. Use FocusChanged instead. " +
            "Like: " +
            "NUIApplication.GetDefaultWindow().FocusChanged = OnFocusChanged; " +
            "private void OnFocusChanged(object source, Window.FocusChangedEventArgs args) {...}")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<FocusChangedEventArgs> WindowFocusChanged
        {
            add
            {
                if (windowFocusChangedEventHandler2 == null)
                {
                    windowFocusChangedEventCallback2 = OnWindowFocusedChanged2;
                    using WindowFocusSignalType signal = new WindowFocusSignalType(Interop.Window.FocusChangedSignal(SwigCPtr), false);
                    signal?.Connect(windowFocusChangedEventCallback2);
                }
                windowFocusChangedEventHandler2 += value;
            }
            remove
            {
                windowFocusChangedEventHandler2 -= value;
                if (windowFocusChangedEventHandler2 == null && windowFocusChangedEventCallback2 != null)
                {
                    using WindowFocusSignalType signal = new WindowFocusSignalType(Interop.Window.FocusChangedSignal(SwigCPtr), false);
                    signal?.Disconnect(windowFocusChangedEventCallback2);
                    if (signal?.Empty() == true)
                    {
                        windowFocusChangedEventCallback2 = null;
                    }
                }
            }
        }

        /// <summary>
        /// EffectStart
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<TransitionEffectEventArgs> TransitionEffect
        {
            add
            {
                if (transitionEffectHandler == null)
                {
                    transitionEffectEventCallback = OnTransitionEffect;
                    using WindowTransitionEffectSignal signal = new WindowTransitionEffectSignal(Interop.WindowTransitionEffectSignal.GetSignal(SwigCPtr), false);
                    signal?.Connect(transitionEffectEventCallback);
                }
                transitionEffectHandler += value;
            }
            remove
            {
                transitionEffectHandler -= value;
                if (transitionEffectHandler == null && transitionEffectEventCallback != null)
                {
                    using WindowTransitionEffectSignal signal = new WindowTransitionEffectSignal(Interop.WindowTransitionEffectSignal.GetSignal(SwigCPtr), false);
                    signal?.Disconnect(transitionEffectEventCallback);
                    if (signal?.Empty() == true)
                    {
                        transitionEffectEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// MovedEvent
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<WindowMovedEventArgs> Moved
        {
            add
            {
                if (movedHandler == null)
                {
                    movedEventCallback = OnMoved;
                    using WindowMovedSignal signal = new WindowMovedSignal(Interop.WindowMovedSignal.GetSignal(SwigCPtr), false);
                    signal?.Connect(movedEventCallback);
                }
                movedHandler += value;
            }
            remove
            {
                movedHandler -= value;
                if (movedHandler == null && movedEventCallback != null)
                {
                    using WindowMovedSignal signal = new WindowMovedSignal(Interop.WindowMovedSignal.GetSignal(SwigCPtr), false);
                    signal?.Disconnect(movedEventCallback);
                    if (signal?.Empty() == true)
                    {
                        movedEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// Window Orientation Changed event
        /// This event is for per windows
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<WindowOrientationChangedEventArgs> OrientationChanged
        {
            add
            {
                if (orientationChangedHandler == null)
                {
                    orientationChangedEventCallback = OnOrientationChanged;
                    using WindowOrientationChangedSignal signal = new WindowOrientationChangedSignal(Interop.WindowOrientationChangedSignal.GetSignal(SwigCPtr), false);
                    signal?.Connect(orientationChangedEventCallback);
                }
                orientationChangedHandler += value;
            }
            remove
            {
                orientationChangedHandler -= value;
                if (orientationChangedHandler == null && orientationChangedEventCallback != null)
                {
                    using WindowOrientationChangedSignal signal = new WindowOrientationChangedSignal(Interop.WindowOrientationChangedSignal.GetSignal(SwigCPtr), false);
                    signal?.Disconnect(orientationChangedEventCallback);
                    if (signal?.Empty() == true)
                    {
                        orientationChangedEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// Keyboard Repeat Settings Changed
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler KeyboardRepeatSettingsChanged
        {
            add
            {
                if (keyboardRepeatSettingsChangedHandler == null)
                {
                    keyboardRepeatSettingsChangedEventCallback = OnKeyboardRepeatSettingsChanged;
                    using KeyboardRepeatSettingsChangedSignal signal = new KeyboardRepeatSettingsChangedSignal(Interop.KeyboardRepeatSettingsChangedSignal.GetSignal(SwigCPtr), false);
                    signal?.Connect(keyboardRepeatSettingsChangedEventCallback);
                }
                keyboardRepeatSettingsChangedHandler += value;
            }
            remove
            {
                keyboardRepeatSettingsChangedHandler -= value;
                if (keyboardRepeatSettingsChangedHandler == null && keyboardRepeatSettingsChangedEventCallback != null)
                {
                    using KeyboardRepeatSettingsChangedSignal signal = new KeyboardRepeatSettingsChangedSignal(Interop.KeyboardRepeatSettingsChangedSignal.GetSignal(SwigCPtr), false);
                    signal?.Disconnect(keyboardRepeatSettingsChangedEventCallback);
                    if (signal?.Empty() == true)
                    {
                        keyboardRepeatSettingsChangedEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// ViewAdded will be triggered when the view added on Window
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler ViewAdded;
        private event EventHandler<FocusChangedEventArgs> windowFocusChangedEventHandler;
        private event EventHandler<TouchEventArgs> rootLayerTouchDataEventHandler;
        private ReturnTypeEventHandler<object, TouchEventArgs, bool> rootLayerInterceptTouchDataEventHandler;
        private event EventHandler<WheelEventArgs> stageWheelHandler;
        private event EventHandler<KeyEventArgs> stageKeyHandler;
        private ReturnTypeEventHandler<object, KeyEventArgs, bool> stageInterceptKeyHandler;
        private event EventHandler stageEventProcessingFinishedEventHandler;
        private event EventHandler<ResizedEventArgs> windowResizeEventHandler;
        private event EventHandler<FocusChangedEventArgs> windowFocusChangedEventHandler2;
        private event EventHandler<TransitionEffectEventArgs> transitionEffectHandler;
        private event EventHandler<WindowMovedEventArgs> movedHandler;
        private event EventHandler<WindowOrientationChangedEventArgs> orientationChangedHandler;
        private event EventHandler keyboardRepeatSettingsChangedHandler;
        private event EventHandler<AuxiliaryMessageEventArgs> auxiliaryMessageEventHandler;

        internal void SendViewAdded(View view)
        {
            ViewAdded?.Invoke(view, EventArgs.Empty);
        }

        internal event EventHandler EventProcessingFinished
        {
            add
            {
                if (stageEventProcessingFinishedEventHandler == null)
                {
                    stageEventProcessingFinishedEventCallbackDelegate = OnEventProcessingFinished;
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.EventProcessingFinishedSignal(stageCPtr), false);
                    signal?.Connect(stageEventProcessingFinishedEventCallbackDelegate);
                }
                stageEventProcessingFinishedEventHandler += value;
            }
            remove
            {
                stageEventProcessingFinishedEventHandler -= value;
                if (stageEventProcessingFinishedEventHandler == null && stageEventProcessingFinishedEventCallbackDelegate != null)
                {
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.EventProcessingFinishedSignal(stageCPtr), false);
                    signal?.Disconnect(stageEventProcessingFinishedEventCallbackDelegate);
                    if (signal?.Empty() == true)
                    {
                        stageEventProcessingFinishedEventCallbackDelegate = null;
                    }

                }
            }
        }

        internal event EventHandler ContextLost
        {
            add
            {
                if (stageContextLostEventHandler == null)
                {
                    stageContextLostEventCallbackDelegate = OnContextLost;
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.ContextLostSignal(stageCPtr), false);
                    signal?.Connect(stageContextLostEventCallbackDelegate);
                }
                stageContextLostEventHandler += value;
            }
            remove
            {
                stageContextLostEventHandler -= value;
                if (stageContextLostEventHandler == null && stageContextLostEventCallbackDelegate != null)
                {
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.ContextLostSignal(stageCPtr), false);
                    signal?.Disconnect(stageContextLostEventCallbackDelegate);
                    if (signal?.Empty() == true)
                    {
                        stageContextLostEventCallbackDelegate = null;
                    }
                }
            }
        }

        internal event EventHandler ContextRegained
        {
            add
            {
                if (stageContextRegainedEventHandler == null)
                {
                    stageContextRegainedEventCallbackDelegate = OnContextRegained;
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.ContextRegainedSignal(stageCPtr), false);
                    signal?.Connect(stageContextRegainedEventCallbackDelegate);
                }
                stageContextRegainedEventHandler += value;
            }
            remove
            {
                stageContextRegainedEventHandler -= value;
                if (stageContextRegainedEventHandler == null && stageContextRegainedEventCallbackDelegate != null)
                {
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.ContextRegainedSignal(stageCPtr), false);
                    signal?.Disconnect(stageContextRegainedEventCallbackDelegate);
                    if (signal?.Empty() == true)
                    {
                        stageContextRegainedEventCallbackDelegate = null;
                    }
                }
            }
        }

        internal event EventHandler SceneCreated
        {
            add
            {
                if (stageSceneCreatedEventHandler == null)
                {
                    stageSceneCreatedEventCallbackDelegate = OnSceneCreated;
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.SceneCreatedSignal(stageCPtr), false);
                    signal?.Connect(stageSceneCreatedEventCallbackDelegate);
                }
                stageSceneCreatedEventHandler += value;
            }
            remove
            {
                stageSceneCreatedEventHandler -= value;
                if (stageSceneCreatedEventHandler == null && stageSceneCreatedEventCallbackDelegate != null)
                {
                    using VoidSignal signal = new VoidSignal(Interop.StageSignal.SceneCreatedSignal(stageCPtr), false);
                    signal?.Disconnect(stageSceneCreatedEventCallbackDelegate);
                    if (signal?.Empty() == true)
                    {
                        stageSceneCreatedEventCallbackDelegate = null;
                    }
                }
            }
        }

        internal System.IntPtr GetNativeWindowHandler()
        {
            System.IntPtr ret = Interop.Window.GetNativeWindowHandler(HandleRef.ToIntPtr(this.SwigCPtr));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Disconnect all native signals
        /// </summary>
        /// <since_tizen> 5 </since_tizen>
        internal void DisconnectNativeSignals()
        {
            if (HasBody() == false)
            {
                NUILog.Debug($"[Dispose] DisConnectFromSignals() No native body! No need to Disconnect Signals!");
                return;
            }

            if (windowFocusChangedEventCallback != null)
            {
                using WindowFocusSignalType signal = new WindowFocusSignalType(Interop.Window.FocusChangedSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(windowFocusChangedEventCallback);
                windowFocusChangedEventCallback = null;
            }

            if (rootLayerTouchDataCallback != null)
            {
                using TouchDataSignal signal = new TouchDataSignal(Interop.ActorSignal.ActorTouchSignal(Layer.getCPtr(GetRootLayer())), false);
                signal?.Disconnect(rootLayerTouchDataCallback);
                rootLayerTouchDataCallback = null;
            }

            if (rootLayerInterceptTouchDataCallback != null)
            {
                using TouchDataSignal signal = new TouchDataSignal(Interop.ActorSignal.ActorInterceptTouchSignal(Layer.getCPtr(GetRootLayer())), false);
                signal?.Disconnect(rootLayerInterceptTouchDataCallback);
                rootLayerInterceptTouchDataCallback = null;
            }

            if (wheelEventCallback != null)
            {
                using WheelSignal signal = new WheelSignal(Interop.ActorSignal.ActorWheelEventSignal(Layer.getCPtr(this.GetRootLayer())), false);
                signal?.Disconnect(wheelEventCallback);
                wheelEventCallback = null;
            }

            if (DetentEventCallback != null)
            {
                using StageWheelSignal signal = new StageWheelSignal(Interop.StageSignal.WheelEventSignal(stageCPtr), false);
                signal?.Disconnect(DetentEventCallback);
                DetentEventCallback = null;
            }

            if (stageKeyCallbackDelegate != null)
            {
                using KeyEventSignal signal = new KeyEventSignal(Interop.Window.KeyEventSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(stageKeyCallbackDelegate);
                stageKeyCallbackDelegate = null;
            }

            if (stageInterceptKeyCallbackDelegate != null)
            {
                using KeyEventSignal signal = new KeyEventSignal(Interop.Window.InterceptKeyEventSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(stageInterceptKeyCallbackDelegate);
                stageInterceptKeyCallbackDelegate = null;
            }

            if (stageEventProcessingFinishedEventCallbackDelegate != null)
            {
                using VoidSignal signal = new VoidSignal(Interop.StageSignal.EventProcessingFinishedSignal(stageCPtr), false);
                signal?.Disconnect(stageEventProcessingFinishedEventCallbackDelegate);
                stageEventProcessingFinishedEventCallbackDelegate = null;
            }

            if (stageContextLostEventCallbackDelegate != null)
            {
                using VoidSignal signal = new VoidSignal(Interop.StageSignal.ContextLostSignal(stageCPtr), false);
                signal?.Disconnect(stageContextLostEventCallbackDelegate);
                stageContextLostEventCallbackDelegate = null;
            }

            if (stageContextRegainedEventCallbackDelegate != null)
            {
                using VoidSignal signal = new VoidSignal(Interop.StageSignal.ContextRegainedSignal(stageCPtr), false);
                signal?.Disconnect(stageContextRegainedEventCallbackDelegate);
                stageContextRegainedEventCallbackDelegate = null;
            }

            if (stageSceneCreatedEventCallbackDelegate != null)
            {
                using VoidSignal signal = new VoidSignal(Interop.StageSignal.SceneCreatedSignal(stageCPtr), false);
                signal?.Disconnect(stageSceneCreatedEventCallbackDelegate);
                stageSceneCreatedEventCallbackDelegate = null;
            }

            if (windowResizeEventCallback != null)
            {
                using ResizeSignal signal = new ResizeSignal(Interop.Window.ResizeSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(windowResizeEventCallback);
                windowResizeEventCallback = null;
            }

            if (windowFocusChangedEventCallback2 != null)
            {
                using WindowFocusSignalType signal = new WindowFocusSignalType(Interop.Window.FocusChangedSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(windowFocusChangedEventCallback2);
                windowFocusChangedEventCallback2 = null;
            }

            if (transitionEffectEventCallback != null)
            {
                using WindowTransitionEffectSignal signal = new WindowTransitionEffectSignal(Interop.WindowTransitionEffectSignal.GetSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(transitionEffectEventCallback);
                transitionEffectEventCallback = null;
            }

            if (movedEventCallback != null)
            {
                using WindowMovedSignal signal = new WindowMovedSignal(Interop.WindowMovedSignal.GetSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(movedEventCallback);
                movedEventCallback = null;
            }

            if (orientationChangedEventCallback != null)
            {
                using WindowOrientationChangedSignal signal = new WindowOrientationChangedSignal(Interop.WindowOrientationChangedSignal.GetSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(orientationChangedEventCallback);
                orientationChangedEventCallback = null;
            }

            if (keyboardRepeatSettingsChangedEventCallback != null)
            {
                using KeyboardRepeatSettingsChangedSignal signal = new KeyboardRepeatSettingsChangedSignal(Interop.KeyboardRepeatSettingsChangedSignal.GetSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(keyboardRepeatSettingsChangedEventCallback);
                keyboardRepeatSettingsChangedEventCallback = null;
            }

            if (auxiliaryMessageEventCallback != null)
            {
                using WindowAuxiliaryMessageSignal signal = new WindowAuxiliaryMessageSignal(Interop.WindowAuxiliaryMessageSignalType.Get(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(auxiliaryMessageEventCallback);
                auxiliaryMessageEventCallback = null;
            }

            if (AccessibilityHighlightEventCallback != null)
            {
                using WindowAccessibilityHighlightEvent signal = new WindowAccessibilityHighlightEvent(Interop.WindowAccessibilityHighlightSignal.GetSignal(GetBaseHandleCPtrHandleRef), false);
                signal?.Disconnect(AccessibilityHighlightEventCallback);
                AccessibilityHighlightEventCallback = null;
            }
        }

        private void OnWindowFocusedChanged(IntPtr window, bool focusGained)
        {
            if (window == IntPtr.Zero)
            {
                NUILog.Error("OnWindowFocusedChanged() Window is null! Do nothing!");
                return;
            }

            if (windowFocusChangedEventHandler != null)
            {
                FocusChangedEventArgs e = new FocusChangedEventArgs();
                e.FocusGained = focusGained;
                windowFocusChangedEventHandler(this, e);
            }
        }

        private bool OnWindowTouch(IntPtr view, IntPtr touchData)
        {
            if (touchData == global::System.IntPtr.Zero)
            {
                NUILog.Error("touchData should not be null!");
                return false;
            }

            if (rootLayerTouchDataEventHandler != null)
            {
                TouchEventArgs e = new TouchEventArgs();
                e.Touch = Tizen.NUI.Touch.GetTouchFromPtr(touchData);
                rootLayerTouchDataEventHandler(this, e);
            }
            return false;
        }

        private bool OnWindowInterceptTouch(IntPtr view, IntPtr touchData)
        {
            if (touchData == global::System.IntPtr.Zero)
            {
                NUILog.Error("touchData should not be null!");
                return true;
            }

            bool consumed = false;
            if (rootLayerInterceptTouchDataEventHandler != null)
            {
                TouchEventArgs e = new TouchEventArgs();
                e.Touch = Tizen.NUI.Touch.GetTouchFromPtr(touchData);
                consumed = rootLayerInterceptTouchDataEventHandler(this, e);
            }
            return consumed;
        }

        private bool OnStageWheel(IntPtr rootLayer, IntPtr wheelEvent)
        {
            if (wheelEvent == global::System.IntPtr.Zero)
            {
                NUILog.Error("wheelEvent should not be null!");
                return true;
            }

            if (stageWheelHandler != null)
            {
                WheelEventArgs e = new WheelEventArgs();
                e.Wheel = Tizen.NUI.Wheel.GetWheelFromPtr(wheelEvent);
                stageWheelHandler(this, e);
            }
            return true;
        }

        // Callback for Stage KeyEventsignal
        private void OnStageKey(IntPtr data)
        {
            if (stageKeyHandler != null)
            {
                KeyEventArgs e = new KeyEventArgs();
                e.Key = Tizen.NUI.Key.GetKeyFromPtr(data);
                //here we send all data to user event handlers
                stageKeyHandler(this, e);
            }
        }

        // Callback for Stage InterceptKeyEventsignal
        private bool OnStageInterceptKey(IntPtr data)
        {
            bool consumed = false;
            if (stageInterceptKeyHandler != null)
            {
                KeyEventArgs e = new KeyEventArgs();
                e.Key = Tizen.NUI.Key.GetKeyFromPtr(data);
                //here we send all data to user event handlers
                consumed = stageInterceptKeyHandler(this, e);
            }
            return consumed;
        }

        // Callback for Stage EventProcessingFinishedSignal
        private void OnEventProcessingFinished()
        {
            stageEventProcessingFinishedEventHandler?.Invoke(this, null);
        }

        // Callback for Stage ContextLostSignal
        private void OnContextLost()
        {
            stageContextLostEventHandler?.Invoke(this, null);
        }

        // Callback for Stage ContextRegainedSignal
        private void OnContextRegained()
        {
            stageContextRegainedEventHandler?.Invoke(this, null);
        }

        // Callback for Stage SceneCreatedSignal
        private void OnSceneCreated()
        {
            stageSceneCreatedEventHandler?.Invoke(this, null);
        }

        private void OnResized(IntPtr window, IntPtr windowSize)
        {
            if (window == IntPtr.Zero)
            {
                NUILog.Error("OnResized() Window is null! Do nothing!");
                return;
            }

            if (windowResizeEventHandler != null)
            {
                ResizedEventArgs e = new ResizedEventArgs();
                // var val = new Uint16Pair(windowSize, false);
                // e.WindowSize = new Size2D(val.GetWidth(), val.GetHeight());
                // val.Dispose();

                // Workaround : windowSize should be valid pointer from dali,
                // but currently it is fixed and is not Uint16Pair class.
                // will be fixed later.
                e.WindowSize = this.WindowSize;
                windowResizeEventHandler(this, e);
            }
        }

        private void OnWindowFocusedChanged2(IntPtr window, bool focusGained)
        {
            if (window == IntPtr.Zero)
            {
                NUILog.Error("OnWindowFocusedChanged() Window is null! Do nothing!");
                return;
            }

            if (windowFocusChangedEventHandler2 != null)
            {
                FocusChangedEventArgs e = new FocusChangedEventArgs();
                e.FocusGained = focusGained;
                windowFocusChangedEventHandler2(this, e);
            }
        }

        private void OnTransitionEffect(IntPtr window, int state, int type)
        {
            if (window == global::System.IntPtr.Zero)
            {
                return;
            }

            if (transitionEffectHandler != null)
            {
                TransitionEffectEventArgs e = new TransitionEffectEventArgs();
                e.State = (EffectState)state;
                e.Type = (EffectType)type;
                transitionEffectHandler(this, e);
            }
            return;
        }

        private void OnMoved(IntPtr window, IntPtr position)
        {
            if (window == global::System.IntPtr.Zero)
            {
                return;
            }

            if (movedHandler != null)
            {
                WindowMovedEventArgs e = new WindowMovedEventArgs();
                e.WindowPosition = this.WindowPosition;
                movedHandler(this, e);
            }
            return;
        }

        private void OnOrientationChanged(IntPtr window, int orientation)
        {
            if (window == global::System.IntPtr.Zero)
            {
                return;
            }

            if (orientationChangedHandler != null)
            {
                WindowOrientationChangedEventArgs e = new WindowOrientationChangedEventArgs();
                e.WindowOrientation = (WindowOrientation)orientation;
                orientationChangedHandler(this, e);
            }
            return;
        }

        private void OnKeyboardRepeatSettingsChanged()
        {
            keyboardRepeatSettingsChangedHandler?.Invoke(this, null);
            return;
        }

        /// <summary>
        /// The focus changed event argument.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class FocusChangedEventArgs : EventArgs
        {
            /// <summary>
            /// FocusGained flag.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public bool FocusGained
            {
                get;
                set;
            }
        }

        /// <summary>
        /// The touch event argument.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class TouchEventArgs : EventArgs
        {
            private Touch touch;

            /// <summary>
            /// Touch.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public Touch Touch
            {
                get
                {
                    return touch;
                }
                set
                {
                    touch = value;
                }
            }
        }

        /// <summary>
        /// Wheel event arguments.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class WheelEventArgs : EventArgs
        {
            private Wheel wheel;

            /// <summary>
            /// Wheel.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public Wheel Wheel
            {
                get
                {
                    return wheel;
                }
                set
                {
                    wheel = value;
                }
            }
        }

        /// <summary>
        /// Key event arguments.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class KeyEventArgs : EventArgs
        {
            private Key key;

            /// <summary>
            /// Key.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public Key Key
            {
                get
                {
                    return key;
                }
                set
                {
                    key = value;
                }
            }
        }

        /// <summary>
        /// Feeds a key event into the window.
        /// This resized event arguments.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public class ResizedEventArgs : EventArgs
        {
            Size2D windowSize;

            /// <summary>
            /// This window size.
            /// </summary>
            /// <since_tizen> 4 </since_tizen>
            public Size2D WindowSize
            {
                get
                {
                    return windowSize;
                }
                set
                {
                    windowSize = value;
                }
            }
        }

        /// <summary>
        /// Do not use this, that will be deprecated.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [Obsolete("Do not use this, that will be deprecated. Use FocusChangedEventArgs instead. " +
            "Like: " +
            "NUIApplication.GetDefaultWindow().FocusChanged = OnFocusChanged; " +
            "private void OnFocusChanged(object source, Window.FocusChangedEventArgs args) {...}")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class WindowFocusChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Do not use this, that will be deprecated.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            public bool FocusGained
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Contains and encapsulates Native Window handle.
        /// </summary>
        /// <since_tizen> 4 </since_tizen>
        [Obsolete("This has been deprecated in API9 and will be removed in API11. Use Window.NativeHandle instead.")]
        public class SafeNativeWindowHandle : SafeHandle
        {
            /// <summary>
            ///Constructor, Native window handle is set to handle.
            /// </summary>
            /// <since_tizen> 4 </since_tizen>
            public SafeNativeWindowHandle() : base(IntPtr.Zero, false)
            {
                SetHandle(NUIApplication.GetDefaultWindow().GetNativeWindowHandler());
            }
            /// <summary>
            /// Null check if the handle is valid or not.
            /// </summary>
            /// <since_tizen> 4 </since_tizen>
            public override bool IsInvalid
            {
                get
                {
                    return this.handle == IntPtr.Zero;
                }
            }
            /// <summary>
            /// Release handle itself.
            /// </summary>
            /// <returns>true when released successfully.</returns>
            /// <since_tizen> 4 </since_tizen>
            protected override bool ReleaseHandle()
            {
                return true;
            }
        }

        /// <summary>
        /// TransitionEffectArgs
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class TransitionEffectEventArgs : EventArgs
        {
            private EffectState state;
            private EffectType type;

            /// <summary>
            /// State
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public EffectState State
            {
                get
                {
                    return state;
                }
                set
                {
                    state = value;
                }
            }
            /// <summary>
            /// Type
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public EffectType Type
            {
                get
                {
                    return type;
                }
                set
                {
                    type = value;
                }
            }
        }

        private EventHandler<WheelEventArgs> DetentEventHandler;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DetentEventCallbackType(IntPtr arg1);

        private DetentEventCallbackType DetentEventCallback;

        private void OnDetentEvent(IntPtr wheelEvent)
        {
            WheelEventArgs e = new WheelEventArgs();

            if (wheelEvent != global::System.IntPtr.Zero)
            {
                e.Wheel = Wheel.GetWheelFromPtr(wheelEvent);
            }

            DetentEventHandler?.Invoke(this, e);
        }

        /// <summary>
        /// VisibilityChangedArgs
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class VisibilityChangedEventArgs : EventArgs
        {
            private bool visibility;
            /// <summary>
            /// Visibility
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public bool Visibility
            {
                get => visibility;
                set
                {
                    visibility = value;
                }
            }
        }

        private void OnVisibilityChanged(IntPtr window, bool visibility)
        {
            if (window == global::System.IntPtr.Zero)
            {
                NUILog.Error("[ERR] OnVisibilityChanged() window is null");
                return;
            }

            VisibilityChangedEventArgs e = new VisibilityChangedEventArgs();
            e.Visibility = visibility;
            if (VisibilityChangedEventHandler != null)
            {
                VisibilityChangedEventHandler.Invoke(this, e);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void VisibilityChangedEventCallbackType(IntPtr window, bool visibility);
        private VisibilityChangedEventCallbackType VisibilityChangedEventCallback;
        private event EventHandler<VisibilityChangedEventArgs> VisibilityChangedEventHandler;
        private WindowVisibilityChangedEvent VisibilityChangedEventSignal;

        /// <summary>
        /// EffectStart
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<VisibilityChangedEventArgs> VisibilityChanged
        {
            add
            {
                // This is to defend against problems in the operation of unsubscribing the VisibilityChanged event of the sub window within the handler,
                // where the sub window is disabled and the View RemovedFromWindow event occurs.
                // The problem seems to have occurred because the native window object itself is invalid 
                // when trying to obtain the window native signal because the sub window is in the Dispose state.
                if (Disposed)
                {
                    Tizen.Log.Error("NUI", "[ERR] This Window object has already been destroyed, but VisibilityChanged event is added or removed!");
                    return;
                }

                if (VisibilityChangedEventHandler == null)
                {
                    VisibilityChangedEventCallback = OnVisibilityChanged;
                    using WindowVisibilityChangedEvent signal = new WindowVisibilityChangedEvent(Interop.WindowVisibilityChangedSignal.GetSignal(Window.getCPtr(this)), false);
                    signal?.Connect(VisibilityChangedEventCallback);
                }
                VisibilityChangedEventHandler += value;
            }
            remove
            {
                // This is to defend against problems in the operation of unsubscribing the VisibilityChanged event of the sub window within the handler,
                // where the sub window is disabled and the View RemovedFromWindow event occurs.
                // The problem seems to have occurred because the native window object itself is invalid 
                // when trying to obtain the window native signal because the sub window is in the Dispose state.
                if (Disposed)
                {
                    Tizen.Log.Error("NUI", "[ERR] This Window object has already been destroyed, but VisibilityChanged event is added or removed!");
                    return;
                }

                VisibilityChangedEventHandler -= value;
                if (VisibilityChangedEventHandler == null && VisibilityChangedEventCallback != null)
                {
                    using WindowVisibilityChangedEvent signal = new WindowVisibilityChangedEvent(Interop.WindowVisibilityChangedSignal.GetSignal(Window.getCPtr(this)), false);
                    signal?.Disconnect(VisibilityChangedEventCallback);
                    if (signal?.Empty() == true)
                    {
                        VisibilityChangedEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// VisibiltyChangedSignalEmit
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void VisibiltyChangedSignalEmit(bool visibility)
        {
            if (VisibilityChangedEventSignal == null)
            {
                VisibilityChangedEventSignal = new WindowVisibilityChangedEvent(this);
            }
            VisibilityChangedEventSignal.Emit(this, visibility);
        }

        private void OnAuxiliaryMessage(IntPtr kData, IntPtr vData, IntPtr optionsArray)
        {
            if (kData == IntPtr.Zero || vData == IntPtr.Zero)
            {
                return;
            }

            using var tmp = new PropertyArray(optionsArray, false);
            var size = tmp.Size();

            List<string> tmpList = new List<string>();

            for (int i = 0; i < size; i++)
            {
                string option = "none";
                tmp.GetElementAt((uint)i).Get(out option);
                tmpList.Add(option);
            }

            tmp.Dispose();

            AuxiliaryMessageEventArgs e = new AuxiliaryMessageEventArgs();
            e.Key = StringToVoidSignal.GetResult(kData);
            e.Value = StringToVoidSignal.GetResult(vData); ;
            e.Options = tmpList;

            auxiliaryMessageEventHandler?.Invoke(this, e);
        }

        /// <summary>
        /// Auxiliary message is sent by displayer server when window's auxiliary was changed then display server sent the message.
        /// When client application added the window's auxiliary hint and if the auxiliary is changed,
        /// display server send the auxiliary message.
        /// Auxiliary message has the key, value and options.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<AuxiliaryMessageEventArgs> AuxiliaryMessage
        {
            add
            {
                if (auxiliaryMessageEventHandler == null)
                {
                    auxiliaryMessageEventCallback = OnAuxiliaryMessage;
                    using WindowAuxiliaryMessageSignal signal = new WindowAuxiliaryMessageSignal(Interop.WindowAuxiliaryMessageSignalType.Get(SwigCPtr), false);
                    signal?.Connect(auxiliaryMessageEventCallback);
                }
                auxiliaryMessageEventHandler += value;
            }
            remove
            {
                auxiliaryMessageEventHandler -= value;
                if (auxiliaryMessageEventHandler == null && auxiliaryMessageEventCallback != null)
                {
                    using WindowAuxiliaryMessageSignal signal = new WindowAuxiliaryMessageSignal(Interop.WindowAuxiliaryMessageSignalType.Get(SwigCPtr), false);
                    signal?.Disconnect(auxiliaryMessageEventCallback);
                    if (signal?.Empty() == true)
                    {
                        auxiliaryMessageEventCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// AccessibilityHighlightArgs
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class AccessibilityHighlightEventArgs : EventArgs
        {
            private bool accessibilityHighlight;
            /// <summary>
            /// accessibilityHighlight
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public bool AccessibilityHighlight
            {
                get => accessibilityHighlight;
                set
                {
                    accessibilityHighlight = value;
                }
            }
        }

        private void OnAccessibilityHighlight(IntPtr window, bool highlight)
        {
            if (window == global::System.IntPtr.Zero)
            {
                NUILog.Error("[ERR] OnAccessibilityHighlight() window is null");
                return;
            }

            AccessibilityHighlightEventArgs e = new AccessibilityHighlightEventArgs();
            e.AccessibilityHighlight = highlight;
            if (AccessibilityHighlightEventHandler != null)
            {
                AccessibilityHighlightEventHandler.Invoke(this, e);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void AccessibilityHighlightEventCallbackType(IntPtr window, bool highlight);
        private AccessibilityHighlightEventCallbackType AccessibilityHighlightEventCallback;
        private event EventHandler<AccessibilityHighlightEventArgs> AccessibilityHighlightEventHandler;

        /// <summary>
        /// Emits the event when the window needs to grab or clear highlight.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<AccessibilityHighlightEventArgs> AccessibilityHighlight
        {
            add
            {
                if (AccessibilityHighlightEventHandler == null)
                {
                    AccessibilityHighlightEventCallback = OnAccessibilityHighlight;
                    using WindowAccessibilityHighlightEvent signal = new WindowAccessibilityHighlightEvent(Interop.WindowAccessibilityHighlightSignal.GetSignal(SwigCPtr), false);
                    signal?.Connect(AccessibilityHighlightEventCallback);
                }
                AccessibilityHighlightEventHandler += value;
            }
            remove
            {
                AccessibilityHighlightEventHandler -= value;
                if (AccessibilityHighlightEventHandler == null && AccessibilityHighlightEventCallback != null)
                {
                    using WindowAccessibilityHighlightEvent signal = new WindowAccessibilityHighlightEvent(Interop.WindowAccessibilityHighlightSignal.GetSignal(SwigCPtr), false);
                    signal?.Disconnect(AccessibilityHighlightEventCallback);
                    if (signal?.Empty() == true)
                    {
                        AccessibilityHighlightEventCallback = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// MovedArgs
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class WindowMovedEventArgs : EventArgs
    {
        private Position2D position;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Position2D WindowPosition
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
    }

    /// <summary>
    /// OrientationChangedArgs
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class WindowOrientationChangedEventArgs : EventArgs
    {
        private Window.WindowOrientation orientation;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Window.WindowOrientation WindowOrientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;
            }
        }
    }
}
