using System.Collections.Generic;
using bSpin.CustomTypes;

namespace bSpin.Network {
    public enum Action {
        ClearQueue,
        StopCurrent
    }

    public struct WebsocketMessage {
        public string Sender;
        public Action? WobbleAction;
        public Action? SpinAction;
        public List<Wobble>? WobbleQueueAdd;
        public List<Spin>? SpinQueueAdd;
    }
}