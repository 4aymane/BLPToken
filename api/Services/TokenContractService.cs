using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Web3;

namespace api.Services
{
    public class TokenContractService
    {
        private const string ContractAddress = "0xfE1d7f7a8f0bdA6E415593a2e4F82c64b446d404";
        private const string RpcUrl = "https://bsc-dataseed.binance.org/";

        [Function("balanceOf", "uint256")]
        public class BalanceOfFunction : FunctionMessage
        {
            [Parameter("address", "_owner", 1)]
            public string Owner { get; set; } = string.Empty;
        }

        [Function("totalSupply", "uint256")]
        public class TotalSupplyFunction : FunctionMessage { }

        [Function("name", "string")]
        public class NameFunction : FunctionMessage { }

        public static async Task<BigInteger> GetTotalSupplyAsync()
        {
            var web3 = new Web3(RpcUrl);
            var totalSupplyHandler = web3.Eth.GetContractQueryHandler<TotalSupplyFunction>();
            return await totalSupplyHandler.QueryAsync<BigInteger>(ContractAddress);
        }

        public static async Task<string> GetTokenNameAsync()
        {
            var web3 = new Web3(RpcUrl);
            var nameHandler = web3.Eth.GetContractQueryHandler<NameFunction>();
            return await nameHandler.QueryAsync<string>(ContractAddress);
        }

        // Method for getting the token balance from the blockchain
        public static async Task<BigInteger> GetTokenBalanceFromBlockchain(string address)
        {
            var web3 = new Web3(RpcUrl);
            var balanceOfFunctionMessage = new BalanceOfFunction() { Owner = address };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            return await balanceHandler.QueryAsync<BigInteger>(
                ContractAddress,
                balanceOfFunctionMessage
            );
        }
    }
}
