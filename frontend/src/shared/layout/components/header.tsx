import React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { Box, Button } from '@mui/material';

const Header = () => {
  return (
    <AppBar position="static">
      <Toolbar sx={{ display: 'flex', justifyContent: 'space-between'}}>
        <Box>
            <Button color="inherit">Events</Button>
        </Box>
        <Box>
            <Button color="inherit">Login</Button>
            <Button color="inherit">Register</Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
