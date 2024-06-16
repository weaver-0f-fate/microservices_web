import { useCallback } from "react"
import { useSecureFetch } from "../../../shared/fetch/use-fetch";

export const useTestAlgorithms = () => {
    const fetch = useSecureFetch();
    
    return useCallback(
        () : Promise<AlgorithmResponse[]> => 
            fetch(`/algorithms`, {
                method: 'GET',
                nokNotification: "Get event request failed"
        }), [])
}

export interface AlgorithmResponse {
    uuid: string;
    name: string;
}