import React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import { Box, Button, Link } from '@mui/material';
import Login from '../../auth/login';
import { useAuth } from 'react-oidc-context';
import Logout from '../../auth/logout';

const Header = () => {
  const auth = useAuth();

  return (
    <AppBar position="static">
      <Toolbar sx={{ display: 'flex', justifyContent: 'space-between'}}>
        <Box>
          <Link href="/events" color="inherit" sx={{ pl: 2, pr: 2 }}>Events</Link>
          {
            auth.isAuthenticated
            ? <Link href="/algorithms" color="inherit" sx={{ pl: 2, pr: 2 }}>Algorithms</Link>
            : null
          }
        </Box>
        <Box>
          {
            auth.isAuthenticated 
            ? <Logout />
            : <Login />
          }
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
