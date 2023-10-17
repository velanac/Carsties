'use client';

import { Pagination } from 'flowbite-react';
import { useState } from 'react';

type Props = {
  currentPage: number;
  pageCount: number;
  pageChange: (page: number) => void;
};

export default function AppPagination(props: Props) {
  const { currentPage, pageCount, pageChange } = props;

  return (
    <Pagination
      currentPage={currentPage}
      onPageChange={(e) => pageChange(e)}
      totalPages={pageCount}
      layout='pagination'
      showIcons={true}
      className='text-blue-500 mb-5'
    />
  );
}
