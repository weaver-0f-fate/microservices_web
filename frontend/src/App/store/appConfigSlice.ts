import { createSlice } from "@reduxjs/toolkit";
import { AppConfiguration } from "../types/appConfiguration";


type setAppConfigAction = {
    type: string,
    payload: AppConfiguration
}

const initialState : AppConfiguration = {
    environment: '',
    keycloakConf: {
        authority: '',
        client_id: '',
        redirect_uri: '',
        onSigninCallback: () => {},
        store: {}
    }
}

export const appConfigSlice = createSlice({
    name: 'appConfig',
    initialState: initialState,
    reducers: {
        setAppConfig: (state: AppConfiguration, action: setAppConfigAction) => {
            state.environment = action.payload.environment;
            state.environmentDisplayName = action.payload.environmentDisplayName;
            state.version = action.payload.version;
            state.keycloakConf = action.payload.keycloakConf;
        }
    }
});

export const {
    setAppConfig
} = appConfigSlice.actions;

export default appConfigSlice.reducer;