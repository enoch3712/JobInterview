using FluentValidation;
using FluentValidation.Validators;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.July2021.Web.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IGetAssetsService _getAssetsService;

        public UserValidator(IGetAssetsService getAssetsService)
        {
            _getAssetsService = getAssetsService;

            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Age).GreaterThan(18);
            RuleFor(x => x.FirstName).MinimumLength(3);
            RuleFor(x => x.LastName).MinimumLength(3);
            RuleFor(x => x.Address).Custom(AddressValidator);
            RuleFor(x => x.Email)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Email address is required")
                    .EmailAddress()
                    .SetValidator(new RegularExpressionValidator<User>("\\@"));

            RuleFor(x => x.Assets).NotNull().CustomAsync(AssetsValidator);
        }

        private async Task AssetsValidator(IList<Asset> inputAssets, ValidationContext<User> userContext, CancellationToken cancellationToken)
        {
           var assets = await _getAssetsService.ExecuteAsync();

           if(inputAssets.Count(e => assets.Any(x => e.Id == x.Id)) != inputAssets.Count)
                userContext.AddFailure("One of the assets is not present");
        }

        private void AddressValidator(string inputText, ValidationContext<User> userContext)
        {
            if (int.TryParse(inputText, out _))
                return;

            var address = inputText.Split(',');

            if (address.Length != 3) {
                userContext.AddFailure("The Address doesn't match the right format");
            }

            if (!IsValidPostalCode(address[0]))
                userContext.AddFailure("The postal is not in the correct format");

            if (!IsValidStreet(address[1]))
                userContext.AddFailure("The street is not in the correct format");

            if (!IsValidHouseNumber(address[2]))
                userContext.AddFailure("The house number is not in the correct format");

        }

        //TODO
        private bool IsValidHouseNumber(string s)
        {
            return true;
        }


        //TODO
        private bool IsValidStreet(string s)
        {
            return true;
        }

        //TODO
        private bool IsValidPostalCode(string s)
        {
            return true;
        }
    }
}
