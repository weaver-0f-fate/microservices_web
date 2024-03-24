import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from '../../shared/layout/layout';
import EventsCalendar from '../../modules/events/eventsCalendar';

const AppRouter = () => {
  return (
    <Layout>
      <Router>
        <Routes>
          <Route path="/" element={<EventsCalendar key={"1"}/> } />
          <Route path="/login" element={<></>} />
          <Route path="/register" element={<></>} />
        </Routes>
      </Router>
    </Layout>
  );
};

export default AppRouter;
