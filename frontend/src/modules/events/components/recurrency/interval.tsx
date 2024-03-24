import React, { useRef } from 'react';
import { InputLabel, Grid, TextField } from '@mui/material';


interface IntervalProps {
    setInterval: any;
    defaultValue: any;
}

const Interval = (props: IntervalProps) => {
    const { setInterval, defaultValue } = props;
    const inputRef = useRef<any>();

    const handleValueChange = (event: any) => {
        setInterval(event.target.value);
    };

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="interval-input">Interval</InputLabel>
            <TextField
                id="interval-input"
                fullWidth
                type='number'
                defaultValue={defaultValue}
                inputRef={inputRef}
                onChange={handleValueChange}
                inputProps={{ min: 0, max: 10, step: 1 }}
            />
        </Grid>
    )
}

export default Interval;