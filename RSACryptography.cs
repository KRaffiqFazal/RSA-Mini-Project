using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks; 

namespace RSA_Mini_Project
{
  public class RSACryptography
  {
    private const int LOWER_BOUND_BITS = 1;
    private const int UPPER_BOUND_BITS = 5;
    private const int PRIMALITY_REPETITIONS = 40;
    public static BigInteger RandomBigValue(int byteArrayLength = 5)
    {
      if(byteArrayLength <= 1)
      {
        byteArrayLength = 2;
      }

      byte[] _randomBytes = new byte[byteArrayLength];

      RandomNumberGenerator.Fill(_randomBytes);

      _randomBytes[0] = 1;
      _randomBytes[_randomBytes.Length - 1] = 0;
      return new BigInteger(_randomBytes);
    }

    private static BigInteger RandomIntegerInRange(BigInteger belowThis) // https://stackoverflow.com/questions/17357760/how-can-i-generate-a-random-biginteger-within-a-certain-range
    {
      byte[] _bytesOfArg = belowThis.ToByteArray();
      BigInteger _result = 0;
      Random _randomGen = new Random();
      do
      {
        _randomGen.NextBytes(_bytesOfArg);
        _bytesOfArg[_bytesOfArg.Length - 1] &= (byte)0x7F;
        _result = new BigInteger(_bytesOfArg);
      }
      while(_result >= belowThis);
      return _result;
    }
    public static bool MillerRabinTest(BigInteger testingValue, BigInteger accuracy) // https://rosettacode.org/wiki/Miller%E2%80%93Rabin_primality_test#C.23
    {
      if(testingValue == 2)
      {
        return true;
      }

      if(testingValue == 1)
      {
        return false;
      }

      BigInteger _testingStep = testingValue - 1;

      while(_testingStep % 2 == 0)
      {
        _testingStep /= 2;
      }
      BigInteger _valA;
      BigInteger _valB;
      BigInteger _valC;
      for(int i = 0; i < accuracy; i++)
      {
        _valA = RandomIntegerInRange(testingValue - 1) + 1;
        _valB = _testingStep;
        _valC = 1;

        for(int j = 0; j < _valB; ++j)
        {
          _valC = (_valC * _valA) % testingValue;
        }

        while(_valB != testingValue - 1 && _valC != 1 && _valC != testingValue - 1)
        {
          _valC = (_valC * _valC) % testingValue;
          _valB *= 2;
        }
        if(_valC != testingValue - 1 && _valB % 2 == 0)
        {
          return false;
        }
      }
      return true;
    }
    public static bool IsPrime(BigInteger testingVal, int accuracy)
    {
      return MillerRabinTest(testingVal, accuracy);
    }
    public static BigInteger RandomPrimeValue()
    {
      BigInteger _primeValue;
      Random _rndBits = new Random();
      while(true)
      {
        _primeValue = RandomBigValue(_rndBits.Next(LOWER_BOUND_BITS, UPPER_BOUND_BITS));
        if(IsPrime(_primeValue, PRIMALITY_REPETITIONS))
        {
          break;
        }
      }
      return _primeValue;
    }
    public static BigInteger TotientFinder(BigInteger primeVal1, BigInteger primeVal2)
    {
      return (primeVal1 - 1) * (primeVal2 - 1);
    }
    public static bool IsCoPrime(BigInteger val1, BigInteger val2)
    {
      bool _returnVal = false;
      if(HCF(val1, val2) == 1)
      {
        _returnVal = true;
      }
      return _returnVal;
    }
    private static BigInteger HCF(BigInteger val1, BigInteger val2)
    {
      while(val1 != 0 && val2 != 0)
      {
        if(val1 > val2)
        {
          val1 = val1 % val2;
        }
        else
        {
          val2 = val2 % val1;
        }
      }
      return val1 | val2;
    }
    public static BigInteger PublicKeyObtainer(BigInteger primeVal1, BigInteger primeVal2)
    {
      BigInteger _validPublicKey = 0;
      BigInteger _totient = TotientFinder(primeVal1, primeVal2);
      for(int i = 2; i <= _totient; i++)
      {
        if(IsCoPrime(_totient, i))
        {
          _validPublicKey = i;
          break;
        }
      }
      return _validPublicKey;
    }
    public static bool IsPublicKey(BigInteger publicKey, BigInteger primeVal1, BigInteger primeVal2)
    {
      bool _returnVal = false;
      if(publicKey > 1 && IsCoPrime(TotientFinder(primeVal1, primeVal2), publicKey))
      {
        _returnVal = true;
      }
      return _returnVal;
    }
    public static BigInteger PrivateKeyObtainer(BigInteger publicKey, BigInteger totient)
    {
      int i = 1;
      while((totient * i + 1) % publicKey != 0)
      {
        i++;
      }
      return (totient * i + 1) / publicKey;
    }

    public static BigInteger Encrypt(BigInteger plainData, BigInteger publicKey, BigInteger primeProduct)
    {
      return BigInteger.ModPow(plainData, publicKey, primeProduct);
    }
    public static BigInteger Decrypt(BigInteger encryptedData, BigInteger privateKey, BigInteger primeProduct) 
    {
      return BigInteger.ModPow(encryptedData, privateKey, primeProduct);
    }

    public static BigInteger PrimeProduct(BigInteger primeVal1, BigInteger primeVal2) 
    {
      return primeVal1 * primeVal2;
    }
  }
}
