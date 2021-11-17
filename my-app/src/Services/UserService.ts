import AssetDto from "../DTOs/AssetDto";
import { connection } from "./hub";
import axios from 'axios';
import User from "../DTOs/User";
import ODataRequest from "../DTOs/ODataRequest";
import ServiceExtensions from "./serviceExtensions";

// should be on webpack externals
const apiUrl = 'https://localhost:44332'

const UsersService = {
  post,
  get,
  remove,
  put
};

// would a piece of software someone else that would save the bearer token on the localstore for example 
function authHeader() {
    return {};
}

function post(user : User) : Promise<User> {
    return new Promise(async (resolve, reject) => {
        try {
            const request = await axios.post<User>(`${apiUrl}/api/user`, user);

            resolve(request.data);
        } catch (err) {
            reject(err)
        }
    });
}

function get() : Promise<User[]> {
    return new Promise(async (resolve, reject) => {
        try {
            const request = await axios.get<ODataRequest<User>>(`${apiUrl}/api/user?$expand=assets`);
            
            //you can configure this on the backend to have a normal entity (1:N)
            //i keept this way to test how odata would
            //respond dynamicly without EDM defenition

            request
                .data
                .$values
                .forEach(u => {
                    const assets = (u["Assets"] as any).$values
                    u.Assets = assets;
                });

            let y = ServiceExtensions.toCamel(request.data.$values);

            resolve(y as User[]);
        } catch (err) {
            reject(err)
        }
    });
}

function remove(id) : Promise<void> {
    return new Promise(async (resolve, reject) => {
        try {
            const request = await axios.delete(`${apiUrl}/api/user/${id}`);

            resolve(request.data);
        } catch (err) {
            reject(err)
        }
    });
}

function put(id, user: User) : Promise<void> {
    return new Promise(async (resolve, reject) => {
        try {
            const request = await axios.put(`${apiUrl}/api/user/${id}`, user);

            resolve(request.data);
        } catch (err) {
            reject(err)
        }
    });
}

export default UsersService;