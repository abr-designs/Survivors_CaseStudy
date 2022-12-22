using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors
{
    public static class Input
    {
        private static bool _setup;

        public static SInput SInput
        {
            get
            {
                if (_setup)
                    return _sInput;
                _sInput = new SInput();
                _setup = true;

                return _sInput;
            }
        }
        private static SInput _sInput;
    }
}
