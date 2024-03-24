import React from 'react';
import Layout from '../shared/layout/layout';
import EventsCalendar from '../modules/events/eventsCalendar';
import nestComponents from '../shared/utilities/nest-components';
import theme from './theme';
import { ThemeProvider } from '@mui/material';
import { Provider as EventProvider } from '../modules/events/store/store';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import AppRouter from './routing/appRouting';

const AppProviders = nestComponents([
  (props: any) => <ThemeProvider theme={theme}>{props.children}</ThemeProvider>,
  (props: any) => <EventProvider>{props.children}</EventProvider>,
  (props: any) => <Layout>{props.children}</Layout>,
]);

const App = () => {

  const router = createBrowserRouter([
    {
      path: "/",
      element: <EventsCalendar key={"1"}/>,
    },
  ]);

  return (
    <ThemeProvider theme={theme}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Layout>
          <RouterProvider router={router}/>
        </Layout>
      </LocalizationProvider>
    </ThemeProvider>
  );
}

export default App;
