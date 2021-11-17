using Hahn.ApplicationProcess.July2021.Data.Repository;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Implementations
{
    public class UpdateService : IUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UpdateService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public bool Execute(int id, User user)
        {
            if (_userRepository.GetById(id) is null)
                return false;

            _userRepository.Update(user);

            _unitOfWork.Commit();

            return true;
        }
    }
}