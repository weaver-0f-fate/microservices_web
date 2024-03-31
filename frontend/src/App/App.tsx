import React, { FunctionComponent, PropsWithChildren } from 'react';
import Layout from '../shared/layout/layout';
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
import AppRouter from './routing/appRouting';
import { SnackbarProvider } from 'notistack';
import { Provider as ReduxProvider } from 'react-redux'
import store from './redux/store'

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
  (props: any) => <Layout>{props.children}</Layout>,
]) as FunctionComponent<PropsWithChildren>;

const App = () => {

  const router = createBrowserRouter([
    {
      path: "/",
      element: <EventsCalendar key={"1"}/>,
    },
  ]);

  return (
    <AppProviders>
        <RouterProvider router={router}/>
    </AppProviders>
  );
}

export default App;
