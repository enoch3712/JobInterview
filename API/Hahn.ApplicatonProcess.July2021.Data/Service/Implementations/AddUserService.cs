using Hahn.ApplicationProcess.July2021.Data.Repository;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Implementations
{
    public class AddUserService : IAddUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public AddUserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userRepository = userRepository;
        }

        public User Execute(User user)
        {
            user = _userRepository.Add(user);

            _unitOfWork.Commit();

            return user;
        }
    }
}
