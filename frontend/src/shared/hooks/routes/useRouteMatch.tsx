import { AgnosticRouteObject } from '@remix-run/router';
import { matchRoutes, useLocation } from 'react-router-dom';

const useRouteMatch = (routes: AgnosticRouteObject[]): string | undefined  => {
    const location = useLocation()
    const route = matchRoutes(routes, location)
    const subPage = route ? route[0]?.params["*"] ? "/"+route[0]?.params["*"] : "" : ""
    const slashCount = subPage.split('/').length - 1

    if(slashCount > 1) {
        return route?.length ? route[0].route.path : undefined
    }
    return route?.length ? route[0].route.path?.replace("/*", subPage) : undefined
}

export default useRouteMatch;