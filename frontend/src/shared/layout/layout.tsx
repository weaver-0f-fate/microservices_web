// src/components/Layout.js
import React from 'react';
import Header from './components/header';
import { Grid } from '@mui/material';

interface LayoutProps {
    children: React.ReactNode;
}

const Layout = (props : LayoutProps) => {

    const { children } = props;

    return (
    <Grid container direction="column" style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
        <Grid item>
        <   Header />
        </Grid>
        <Grid item xs={12}>
            {children}
        </Grid>
    </Grid>
  );
};

export default Layout;
