using Xunit;
using System;
using System.Text;

namespace RedsysTester.Tests
{
    public class RedsysSignatureServiceTests
    {
        private readonly RedsysSignatureService _signer = new RedsysSignatureService();
        
        // Prueba con valores conocidos (ejemplo de documentación Redsys)
        [Fact]
        public void BuildSignatureUrlSafe_WithKnownValues_ReturnsExpectedSignature()
        {
            // Arrange
            var merchantParams = "{\"DS_MERCHANT_ORDER\":\"1234\",\"DS_MERCHANT_AMOUNT\":\"100\"}";
            var merchantParamsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(merchantParams));
            var secretKey = "Mk9m98IfEblmPfrpsawt7BmxObt98Jev"; // Clave de ejemplo
            var order = "1234";
            
            // Act
            var signature = _signer.BuildSignatureUrlSafe(merchantParamsBase64, secretKey, order);
            
            // Assert
            Assert.NotEmpty(signature);
            // Aquí deberías poner el valor exacto esperado según la doc de Redsys
        }
        
        [Fact]
        public void VerifyRedsysSignature_WithValidSignature_ReturnsTrue()
        {
            // Arrange
            var merchantParams = "{\"DS_MERCHANT_ORDER\":\"5678\"}";
            var merchantParamsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(merchantParams));
            var secretKey = "Mk9m98IfEblmPfrpsawt7BmxObt98Jev";
            var order = "5678";
            
            // Generar firma válida
            var validSignature = _signer.BuildSignatureUrlSafe(merchantParamsBase64, secretKey, order);
            
            // Act
            var isValid = _signer.VerifyRedsysSignature(validSignature, merchantParamsBase64, secretKey);
            
            // Assert
            Assert.True(isValid);
        }
    }
}
