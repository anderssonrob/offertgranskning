using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http.HttpResults;
using Offertgranskning.API.Features.Orders;
using Offertgranskning.API.Shared.Domain.Models;
using Offertgranskning.API.Shared.Exceptions;

namespace Offertgranskning.UnitTests.Endpoints;

public class UploadMetadataTests
{
    private readonly UploadMetadata.UploadMetadataRequestValidator _validator = new();

    [Fact]
    public async Task handler_should_return_ok_when_valid_request()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@acme.it");
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, validCustomer);

        // Act
        var result = await UploadMetadata.Handler(request, CancellationToken.None);

        // Assert
        Assert.IsType<Ok<UploadMetadata.UploadMetadataResponse>>(result);
    }
    
    [Fact]
    public async Task handler_should_return_non_null_response_when_valid_request()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@acme.it");
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, validCustomer);

        // Act
        var result = await UploadMetadata.Handler(request, CancellationToken.None);

        // Assert
        var okResult = (Ok<UploadMetadata.UploadMetadataResponse>)result;
        Assert.NotNull(okResult.Value);
    }
    
     
    [Fact]
    public async Task handler_should_return_correct_message_when_valid_request()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@acme.it");
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, validCustomer);

        // Act
        var result = await UploadMetadata.Handler(request, CancellationToken.None);

        // Assert
        var okResult = (Ok<UploadMetadata.UploadMetadataResponse>)result;
        Assert.Equal("The metadata has been uploaded.", okResult.Value?.Message);
    }
    
    [Fact]
    public async Task handler_should_return_non_empty_correlation_id_when_valid_request()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@acme.it");
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, validCustomer);

        // Act
        var result = await UploadMetadata.Handler(request, CancellationToken.None);

        // Assert
        var okResult = (Ok<UploadMetadata.UploadMetadataResponse>)result;
        Assert.NotEqual(Guid.Empty, okResult.Value?.CorrelationId);
    }
    
    [Fact]
    public async Task handler_should_throw_validation_exception_when_invalid_request()
    {
        // Arrange
        var invalidRequest = new UploadMetadata.UploadMetadataRequest((Package)999, null!);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => UploadMetadata.Handler(invalidRequest, CancellationToken.None));
    }
    
    [Theory]
    [InlineData(Package.Basic)]
    [InlineData(Package.Compare)]
    public void validator_should_not_have_error_when_package_is_valid(Package package)
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@came.it");
        var request = new UploadMetadata.UploadMetadataRequest(package, validCustomer);

        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Package);
    }
    
    [Fact]
    public void validator_should_have_error_when_package_is_invalid()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@came.it");
        var request = new UploadMetadata.UploadMetadataRequest((Package)999, validCustomer);

        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Package)
            .WithErrorMessage("Package must be Basic or Compare");
    }
    
    [Fact]
    public void validator_should_have_error_when_customer_is_null()
    {
        // Arrange
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, null!);

        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Customer);
    }
    
    [Fact]
    public void validator_should_have_no_error_when_customer_is_valid()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@acme.it");
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, validCustomer);

        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Customer);
    }

    [Fact]
    public async Task validator_should_have_no_errors_with_a_valid_request()
    {
        // Arrange
        var validCustomer = new Customer("John", "Doe", "john.doe@acme.it");
        var request = new UploadMetadata.UploadMetadataRequest(Package.Basic, validCustomer);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public async Task validator_should_return_errors_when_request_is_invalid() 
    {
        // Arrange
        var request = new UploadMetadata.UploadMetadataRequest((Package)999, null!);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}