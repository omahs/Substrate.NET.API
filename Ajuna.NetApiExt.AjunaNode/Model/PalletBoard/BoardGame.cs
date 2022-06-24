//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Model.Base;
using Ajuna.NetApi.Model.FrameSupport;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace Ajuna.NetApi.Model.PalletBoard
{
    
    
    /// <summary>
    /// >> 209 - Composite[pallet_ajuna_board.BoardGame]
    /// </summary>
    public sealed class BoardGame : BaseType
    {
        
        /// <summary>
        /// >> board_id
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U32 _boardId;
        
        /// <summary>
        /// >> players
        /// </summary>
        private Ajuna.NetApi.Model.FrameSupport.BoundedVecT6 _players;
        
        /// <summary>
        /// >> state
        /// </summary>
        private Ajuna.NetApi.Model.Base.GameState _state;
        
        public Ajuna.NetApi.Model.Types.Primitive.U32 BoardId
        {
            get
            {
                return this._boardId;
            }
            set
            {
                this._boardId = value;
            }
        }
        
        public Ajuna.NetApi.Model.FrameSupport.BoundedVecT6 Players
        {
            get
            {
                return this._players;
            }
            set
            {
                this._players = value;
            }
        }
        
        public Ajuna.NetApi.Model.Base.GameState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }
        
        public override string TypeName()
        {
            return "BoardGame";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(BoardId.Encode());
            result.AddRange(Players.Encode());
            result.AddRange(State.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            BoardId = new Ajuna.NetApi.Model.Types.Primitive.U32();
            BoardId.Decode(byteArray, ref p);
            Players = new Ajuna.NetApi.Model.FrameSupport.BoundedVecT6();
            Players.Decode(byteArray, ref p);
            State = new Ajuna.NetApi.Model.Base.GameState();
            State.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}