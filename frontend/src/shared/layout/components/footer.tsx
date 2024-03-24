import React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

const Footer = () => {
  return (
    <Box
      component="footer"
      sx={{
        backgroundColor: (theme) => theme.palette.primary.main,
        color: (theme) => theme.palette.common.white,
        padding: (theme) => theme.spacing(2),
        textAlign: 'center',
      }}
    >
      <Typography variant="body1">© {new Date().getFullYear()} Events app</Typography>
    </Box>
  );
};

export default Footer;
