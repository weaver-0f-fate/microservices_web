import React from 'react';
import { Select, MenuItem, InputLabel, Grid } from '@mui/material';
import { RRule } from 'rrule'

interface FrequencyProps {
    setFrequency: any;
    defaultValue: any;
}

const Frequency = (props: FrequencyProps) => {
    const { setFrequency, defaultValue } = props;

    const onChange = (event: any) => {
        setFrequency(event.target.value);
    }

    return(
        <Grid item xs={12}>
            <InputLabel htmlFor="frequency-input">Frequency</InputLabel>
            <Select value={defaultValue} onChange={onChange} fullWidth id='frequency-input'>
                <MenuItem key={RRule.DAILY} value={RRule.DAILY}>Daily</MenuItem>
                <MenuItem key={RRule.WEEKLY} value={RRule.WEEKLY}>Weekly</MenuItem>
                <MenuItem key={RRule.MONTHLY} value={RRule.MONTHLY}>Monthly</MenuItem>
                <MenuItem key={RRule.YEARLY} value={RRule.YEARLY}>Yearly</MenuItem>
            </Select>
        </Grid>
    )
}

export default Frequency;