import React, { FunctionComponent, PropsWithChildren, useCallback, useEffect, useMemo, useRef, useState } from 'react';
import EventsCalendar from '../modules/events/eventsCalendar';
import nestComponents from '../shared/utilities/nest-components';
import theme from './theme';
import { ThemeProvider } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import { SnackbarProvider } from 'notistack';
import { Provider as ReduxProvider, useDispatch, useSelector } from 'react-redux'
import store from './redux/store'
import Layout from './layout/layout';
import { AuthProvider, useAuth } from 'react-oidc-context';
import { LoadingState } from '../shared/constants/loadingState';
import { AppConfiguration } from './types/appConfiguration';
import { AppConfigurationResponse, useGetConfiguration } from './fetch/fetchConfiguration';
import { setAppConfig } from './store/appConfigSlice';
import LoadingBackground from './components/loadingBackground';

const NotificationProvider = (props: any) => {
  return <SnackbarProvider {...props} maxSnack={3} sx={{ marginTop: "54px" }} anchorOrigin={{
      vertical: "top",
      horizontal: "right",
  }} />;
};

const AppProviders = nestComponents([
  (props: any) => <LocalizationProvider dateAdapter={AdapterDayjs}>{props.children}</LocalizationProvider>,
  (props: any) => <NotificationProvider>{props.children}</NotificationProvider>,
  (props: any) => <ReduxProvider store={store}>{props.children}</ReduxProvider>,
  (props: any) => <ThemeProvider theme={theme}>{props.children}</ThemeProvider>,
]) as FunctionComponent<PropsWithChildren>;

const App = () => {
  const getConfiguration = useGetConfiguration();
  const dispatch = useDispatch();
  const [loadingState, setLoadingState] = useState(LoadingState.Loading);
  const config = useSelector((state: any) => state.appConfig as AppConfiguration);

  const router = createBrowserRouter([
    {
      path: "/",
      element: <EventsCalendar key={"1"}/>,
    },
  ]);

  const localStore = useRef({
      set: async (key: string, value: string) => {
          localStorage.setItem(key, value);
      },
      get: async (key: string) => {
          return localStorage.getItem(key);
      },
      getAllKeys: async () => {
          let keys: string[] = [];
          for (let i = 0; i < localStorage.length; i++) {
              let key = localStorage.key(i);
              if (key !== null)
                  keys.push();
          }
          return keys;
      },
      remove: async (key: string) => {
          let keyVal = localStorage.getItem(key);
          localStorage.removeItem(key);
          return keyVal;
      },
  });

  const onSignIn = () => {
      window.history.replaceState({}, document.title, window.location.pathname);
  };

  const getRedirectPath = useCallback(() => {
      return window.location.pathname;
  }, []);

  useEffect(() => {
    const redirectPath = getRedirectPath();

    getConfiguration()
        .then((conf: AppConfigurationResponse) => {

            dispatch(setAppConfig({
                environment: conf.Environment,
                environmentDisplayName: conf.EnvironmentDisplayName,
                version: conf.Version,
                keycloakConf: {
                    authority: conf.Keycloak.AuthUrl,
                    client_id: conf.Keycloak.ClientId,
                    redirect_uri: window.location.origin + redirectPath,
                    onSigninCallback: onSignIn,
                    store: localStore.current
                }
            }));

            setLoadingState(LoadingState.Finished);
        })
        .catch((error) => {
            setLoadingState(LoadingState.Error);
        })
  }, []);

  if (loadingState === LoadingState.Finished && config.keycloakConf !== undefined) {
      return (
          <AuthProvider {...config.keycloakConf} userStore={config.keycloakConf.store} stateStore={config.keycloakConf.store}>
              <Layout>
                  <RouterProvider router={router}/>
              </Layout>
          </AuthProvider>
      );
  } else {
      return <LoadingBackground loadingState={loadingState} />
  }
};

const AppWithProviders = () => {
  //enableMapSet(); from immer

  return (
    <AppProviders>
      <App />
    </AppProviders>
  );
}

export default AppWithProviders;
