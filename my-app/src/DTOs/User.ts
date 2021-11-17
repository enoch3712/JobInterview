import Asset from "./Asset";

export default interface User {
    id : number
    age : number
    firstName : string
    lastName : string
    address : string
    email : string
    Assets : Asset[]
}