import React, { useRef } from 'react';
import { InputLabel, Grid } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';

interface EndDateProps {
    setEndDate: any;
    defaultValue: Date | null;
}

const EndDate = (props: EndDateProps) => {
    const { setEndDate, defaultValue } = props;
    const inputRef = useRef<any>();

    const handleDateChange = (event: any) => {
        setEndDate(event.toDate());
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="end-date-input">End Date</InputLabel>
            <DateTimePicker
                sx={{ width: '100%' }}
                defaultValue={dayjs(defaultValue)}
                inputRef={inputRef}
                onChange={handleDateChange}
            />
        </Grid>
    )
}

export default EndDate;