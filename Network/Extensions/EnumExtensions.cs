using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.ActivationFunctions;
using Network.Enums;

namespace Network.Extensions
{
    public static class EnumExtensions
    {
        public static IActivationFunction Initialize(this ActivationFunctionType activationFunctionType) =>
            activationFunctionType switch
            {
                ActivationFunctionType.BinaryStep => new BinaryStep(),
                ActivationFunctionType.EliotSig => new EliotSig(),
                ActivationFunctionType.Gaussian => new Gaussian(),
                ActivationFunctionType.Identity => new Identity(),
                ActivationFunctionType.LeakyReLu => new LeakyRelu(),
                ActivationFunctionType.ReLu => new ReLu(),
                ActivationFunctionType.Sigmoid => new Sigmoid(),
                ActivationFunctionType.SiLu => new SiLu(),
                ActivationFunctionType.SoftPlus => new SoftPlus(),
                ActivationFunctionType.TanH => new Tanh(),
                _ => throw new ArgumentOutOfRangeException(nameof(activationFunctionType), activationFunctionType, null)
            };

        public static IActivationFunction[] InitializeFunctions(this ActivationFunctionType[] activationFunctionTypes)
        {
            IActivationFunction[] res = new IActivationFunction[activationFunctionTypes.Length];


            for (int i = 0; i < res.Length; i++)
            {
                res[i] = activationFunctionTypes[i].Initialize();
            }

            return res;
        }
    }

    public static class ActivationFunctionExtensions
    {
        public static ActivationFunctionType[] ToActivationFunctionTypes(this IActivationFunction[] activationFunctions)
        {
            ActivationFunctionType[] res = new ActivationFunctionType[activationFunctions.Length];


            for (int i = 0; i < res.Length; i++)
            {
                res[i] = activationFunctions[i].EvaluationFunctionId;
            }

            return res;
        }
    }
}
