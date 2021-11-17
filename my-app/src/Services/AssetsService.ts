import AssetDto from "../DTOs/AssetDto";
import { connection } from "./hub";
import axios from 'axios';
import ODataRequest from "../DTOs/ODataRequest";

// should be on webpack externals
const apiUrl = 'https://localhost:44332'

const AssetsService = {
  getAssetsSignalR,
  getAssets
};

// would a piece of software someone else that would save the bearer token on the localstore for example 
function authHeader() {
    return {};
}

//was just a PoC, never had built it from the begining, it works but for this use case is better the normal REST
function getAssetsSignalR() : Promise<AssetDto[]> {
    return new Promise(async (resolve, reject) => {
        try {
            let result = await connection.invoke("Get");

            resolve(result);
        } catch (err) {
            reject(err)
        }
    });
}

function getAssets(top, textSearch = null) : Promise<AssetDto[]> {
    return new Promise(async (resolve, reject) => {
        try {
            var url = `${apiUrl}/api/assets?$top=${top}`

            if(textSearch)
                url += `&$filter=contains(name,'${textSearch}')`;

            let request = await axios.request<ODataRequest<AssetDto>>({url,headers: { ...authHeader()}});

            resolve(request.data.$values);

        } catch (err) {
            reject(err)
        }
    });
}

export default AssetsService;