
export type AppConfiguration = {
    environment?: string,
    environmentDisplayName?: string,
    version?: string,
    keycloakConf: KeycloakConfiguiration,
}

export type KeycloakConfiguiration = {
    authority: string,
    client_id: string,
    redirect_uri: string,
    onSigninCallback: any,
    store: any
}