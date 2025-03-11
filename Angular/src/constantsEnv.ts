import { isDevMode } from "@angular/core";


export class Constants{

    getApiBaseUrl() : string {
        if(isDevMode()){
            return "https://localhost:7283/api";
        }else{
            return "";
            //prod url
        }
    }

}