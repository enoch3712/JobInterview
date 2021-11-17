using Hahn.ApplicationProcess.July2021.Data.Repository;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;

namespace Hahn.ApplicationProcess.July2021.Data.Service.Implementations
{
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public DeleteUserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public bool Execute(int id)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
                return false;

            _userRepository.Delete(user);

            _unitOfWork.Commit();

            return true;
        }
    }
}