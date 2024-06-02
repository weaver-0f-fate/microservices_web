import { useCallback } from "react"
import { useSecureFetch } from "../../../shared/fetch/use-fetch";

export const useTestAlgorithms = () => {
    const fetch = useSecureFetch();
    
    return useCallback(
        () : Promise<any> => 
            fetch(`/algorithms`, {
                method: 'GET',
                nokNotification: "Get event request failed"
        }), [])
}