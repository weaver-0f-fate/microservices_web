import React, { useRef } from 'react';
import { InputLabel, Grid } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';

interface StartDateProps {
    setStartDate: any;
    defaultValue: Date;
}

const StartDate = (props: StartDateProps) => {
    const { setStartDate, defaultValue } = props;
    const inputRef = useRef<any>();

    const handleDateChange = (event: any) => {
        setStartDate(event.toDate());
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="start-date-input">Start Date</InputLabel>
            <DateTimePicker
                sx={{ width: '100%' }}
                defaultValue={dayjs(defaultValue)}
                inputRef={inputRef}
                onChange={handleDateChange}
            />
        </Grid>
    )
}

export default StartDate;