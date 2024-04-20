export type User = {
    isikukood: string,
    eesnimi?: string,
    perenimi?: string,
    õigused: Õigused
}

export type Õigused = {
    rollid: undefined | Set<string>
    liigid: undefined | Set<string>
}
